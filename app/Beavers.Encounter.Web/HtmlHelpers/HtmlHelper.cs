using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcContrib.FluentHtml.Elements;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Web.HtmlHelpers
{
    /// <summary>
    /// Вспомогательный класс для визуализации свойств модели.
    /// </summary>
    public static class HtmlEntityHelper
    {
        /// <summary>
        /// Информация о свойстве модели.
        /// </summary>
        /// <typeparam name="T">Класс объекта модели.</typeparam>
        /// <typeparam name="TV">Тип свойства модели.</typeparam>
        private class PropertyInfo<T, TV>
            where T : Entity
        {
            public string TagId { get; private set; }
            public string TagName { get; private set; }
            public Type ReturnType { get; private set; }
            public object Value { get; private set; }
            public string Caption { get; private set; }
            public string Description { get; private set; }
            public Core.Meta.TextAreaAttribute TextArea { get; private set; }

            public PropertyInfo(T entity, Expression<Func<T, TV>> expr, object defaultValue)
            {
                MemberExpression me = expr.Body is UnaryExpression
                    ? (MemberExpression)((UnaryExpression)expr.Body).Operand
                    : (MemberExpression)expr.Body;

                string entityName = me.Member.ReflectedType.Name;
                string propName = me.Member.Name;
                ReturnType = ((PropertyInfo)me.Member).PropertyType;
                object[] attributes = me.Member.GetCustomAttributes(typeof(Core.Meta.EntityAttribute), true);

                Value = entity != null ? expr.Compile().Invoke(entity) : defaultValue ?? String.Empty;

                Caption = propName;
                Description = String.Empty;

                foreach (var attr in attributes)
                {
                    if (attr is Core.Meta.CaptionAttribute)
                    {
                        Caption = ((Core.Meta.CaptionAttribute)attr).Caption;
                        continue;
                    }
                    if (attr is Core.Meta.DescriptionAttribute)
                    {
                        Description = ((Core.Meta.DescriptionAttribute)attr).Description;
                        continue;
                    }
                    if (attr is Core.Meta.TextAreaAttribute)
                    {
                        TextArea = (Core.Meta.TextAreaAttribute)attr;
                        continue;
                    }
                    if (attr is Core.Meta.DefaultAttribute && entity == null)
                    {
                        Value = ((Core.Meta.DefaultAttribute)attr).Value;
                        continue;
                    }
                }

                TagId = String.Format("{0}_{1}", entityName, propName);
                TagName = String.Format("{0}.{1}", entityName, propName);
            }
        }

        public static string Render<T>(this Entity entity, HtmlHelper html, Expression<Func<T, object>> expr) where T : Entity
        {
            var pi = new PropertyInfo<T, object>((T)entity, expr, null);

            var sb = new StringBuilder();

            sb.Append("<div>");
            sb.AppendFormat("<label for=\"{0}\">{1}:</label>", pi.TagId, pi.Caption);
            if (pi.TextArea != null)
            {
                sb.AppendFormat("<div id=\"{0}\">{1}</div>", pi.TagId, Common.BBCode.ConvertToHtml(html.Encode(pi.Value)));
            }
            else
            {
                string value = Convert.ToString(pi.Value);
                if (pi.ReturnType == typeof(bool))
                {
                    value = Convert.ToBoolean(pi.Value) ? "Да" : "Нет";
                }

                sb.AppendFormat("<span id=\"{0}\">{1}</span>", pi.TagId, html.Encode(value));
            }
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// Визуализация свойства модели для редактирования.
        /// </summary>
        /// <typeparam name="T">Класс объекта модели.</typeparam>
        /// <param name="entity">Объект модели.</param>
        /// <param name="html">Объект для визуализации HTML контролов.</param>
        /// <param name="expr">Выражение для получения значения свойства.</param>
        public static string RenderEditable<T>(this Entity entity, HtmlHelper html, Expression<Func<T, object>> expr) where T : Entity
        {
            return entity.RenderEditable(html, expr, null);
        }

        /// <summary>
        /// Визуализация свойства модели для редактирования.
        /// </summary>
        /// <typeparam name="T">Класс объекта модели.</typeparam>
        /// <param name="entity">Объект модели.</param>
        /// <param name="html">Объект для визуализации HTML контролов.</param>
        /// <param name="expr">Выражение для получения значения свойства.</param>
        /// <param name="defaultValue">Значение по умолчанию, если объект модели равен null.</param>
        public static string RenderEditable<T>(this Entity entity, HtmlHelper html, Expression<Func<T, object>> expr, object defaultValue) where T : Entity
        {
            var pi = new PropertyInfo<T, object>((T)entity, expr, defaultValue);

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"property\">");

            // Чекбокс
            if (pi.ReturnType == typeof(bool))
            {
                CheckBox checkBox = new CheckBox(pi.TagName);
                checkBox.Id(pi.TagId);
                checkBox.Checked(pi.Value.Equals(String.Empty) ? false : Convert.ToBoolean(pi.Value));
                sb.Append("<span class=\"property-value\">");
                sb.Append(checkBox);
                sb.Append("</span>");
                sb.AppendFormat("<label class=\"property-label\" for=\"{0}\">{1}</label>", pi.TagId, pi.Caption);
                sb.Append(html.ValidationMessage(pi.TagName));
            }
            else
            {
                sb.AppendFormat("<label class=\"property-label\" for=\"{0}\">{1}</label>", pi.TagId, pi.Caption);

                sb.Append(html.ValidationMessage(pi.TagName));

                sb.Append("<div class=\"property-value\">");

                if (pi.ReturnType.BaseType == typeof(Entity))
                {

                }
                // Вылетающий список для перечислений 
                else if (pi.ReturnType.BaseType == typeof(Enum))
                {
                    Dictionary<int, string> list = new Dictionary<int, string>();

                    foreach (var item in Enum.GetValues(pi.ReturnType))
                    {
                        list.Add(Convert.ToInt32(item), GetEnumItemDescription(pi.ReturnType, item));
                    }

                    Select select = new Select(pi.TagName);
                    select.Id(pi.TagId);
                    select.Options(list);
                    select.Selected(pi.Value);
                    sb.Append(select);
                }
                // Область ввтода многострочная
                else if (pi.TextArea != null)
                {
                    TextArea textArea = new TextArea(pi.TagName);
                    textArea.Columns(pi.TextArea.Cols);
                    textArea.Rows(pi.TextArea.Rows);
                    textArea.Id(pi.TagId);
                    textArea.Value(pi.Value);
                    sb.Append(textArea);
                    sb.Append(BBCodeNote);
                }
                // Поле ввода однострочное
                else
                {
                    TextBox textBox = new TextBox(pi.TagName);
                    textBox.Id(pi.TagId);
                    textBox.Value(pi.Value);
                    sb.Append(textBox);
                }
                sb.Append("</div>");
            }

            // Описание поля ввода
            if (!String.IsNullOrEmpty(pi.Description))
            {
                sb.AppendFormat("<div class=\"note\">{0}</div>", pi.Description);
            }

            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// Визуализация свойства модели для редактирования.
        /// </summary>
        /// <typeparam name="T">Класс объекта модели.</typeparam>
        /// <typeparam name="TV">Тип значения свойства.</typeparam>
        /// <param name="entity">Объект модели.</param>
        /// <param name="html">Объект для визуализации HTML контролов.</param>
        /// <param name="expr">Выражение для получения значения свойства.</param>
        /// <param name="emptyObject">Значение по умолчанию, если объект модели равен null.</param>
        public static string RenderEditableSingle<T, TV>(this T entity, HtmlHelper html, Expression<Func<T, TV>> expr, IList<TV> allObjects, TV emptyObject) 
            where T : Entity
            where TV : Entity
        {
            var pi = new PropertyInfo<T, TV>(entity, expr, null);

            var sb = new StringBuilder();

            sb.Append("<div class=\"property\">");

            sb.AppendFormat("<label class=\"property-label\" for=\"{0}\">{1}</label>", pi.TagId, pi.Caption);

            sb.Append("<span class=\"property-value\">");

            TV selectedValue = pi.Value != null
                ? (TV)pi.Value
                : emptyObject;
            var list = new SelectList(
                    new List<TV> { selectedValue } 
                    .Union(allObjects.Where(x => x.Id != entity.Id).Except(new List<TV> { (TV)pi.Value }))
                    .Union(new List<TV> { emptyObject }),
                "Id", "Name", pi.Value);

            sb.Append(html.DropDownList(pi.TagName, list));

            sb.Append("</span>");

            if (!String.IsNullOrEmpty(pi.Description))
            {
                sb.AppendFormat("<div class=\"note\">{0}</div>", pi.Description);
            }

            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// Визуализация свойства модели (пречислений) для редактирования.
        /// </summary>
        /// <typeparam name="T">Класс объекта модели.</typeparam>
        /// <typeparam name="TV">Тип свойства модели.</typeparam>
        /// <param name="entity">Объект модели.</param>
        /// <param name="html">Объект для визуализации HTML контролов.</param>
        /// <param name="expr">Выражение для получения значения свойства.</param>
        /// <param name="allObjects">Список из всех элементов.</param>
        /// <param name="emptyObject">Список с одним "пустым" элементом.</param>
        public static string RenderEditable<T, TV>(this T entity, HtmlHelper html, Expression<Func<T, TV>> expr, IList<T> allObjects, T emptyObject) where T : Entity
        {
            var pi = new PropertyInfo<T, TV>(entity, expr, null);
            
            var sb = new StringBuilder();

            sb.Append("<div class=\"property\">");

            sb.AppendFormat("<label class=\"property-label\" for=\"{0}\">{1}</label>", pi.TagId, pi.Caption);

            sb.Append("<span class=\"property-value\">");

            int i = 0;
            foreach (T selectedItem in (IList<T>)pi.Value)
            {
                var list = new SelectList(
                        new List<T> { selectedItem }
                        .Union(allObjects.Where(x => x.Id != entity.Id).Except((IList<T>)pi.Value))
                        .Union(new List<T> { emptyObject }),
                    "Id", "Name", selectedItem);

                sb.Append(html.DropDownList(String.Format("{0}{1}", pi.TagName, i++), list));
            }

            var allList = new SelectList(
                    new List<T> { emptyObject }
                    .Union(allObjects.Where(x => x.Id != entity.Id)
                    .Except((IList<T>)pi.Value)),
                "Id", "Name", emptyObject);

            sb.Append(html.DropDownList(String.Format("{0}{1}", pi.TagName, i), allList));

            sb.Append("</span>");

            if (!String.IsNullOrEmpty(pi.Description))
            {
                sb.AppendFormat("<div class=\"note\">{0}</div>", pi.Description);
            }

            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// Визуализация свойства модели (пречислений) для редактирования.
        /// </summary>
        /// <typeparam name="T">Класс объекта модели.</typeparam>
        /// <typeparam name="TV">Тип свойства модели.</typeparam>
        /// <param name="entity">Объект модели.</param>
        /// <param name="html">Объект для визуализации HTML контролов.</param>
        /// <param name="expr">Выражение для получения значения свойства.</param>
        /// <param name="allObjects">Список из всех элементов.</param>
        /// <param name="emptyObject">Список с одним "пустым" элементом.</param>
        public static string RenderEditableMultiCombo<T, TV, TLI>(this T entity, HtmlHelper html, Expression<Func<T, TV>> expr, IList<TLI> allObjects, TLI emptyObject) where T : Entity
        {
            var pi = new PropertyInfo<T, TV>(entity, expr, null);

            var sb = new StringBuilder();

            sb.Append("<div class=\"property\">");

            sb.AppendFormat("<label class=\"property-label\" for=\"{0}\">{1}</label>", pi.TagId, pi.Caption);

            sb.Append("<span class=\"property-value\">");

            int i = 0;
            foreach (TLI selectedItem in (IList<TLI>)pi.Value)
            {
                var list = new SelectList(
                        new List<TLI> { selectedItem }
                        .Union(allObjects.Except((IList<TLI>)pi.Value))
                        .Union(new List<TLI> { emptyObject }),
                    "Id", "Name", selectedItem);

                sb.Append(html.DropDownList(String.Format("{0}{1}", pi.TagName, i++), list));
            }

            var allList = new SelectList(
                    new List<TLI> { emptyObject }
                    .Union(allObjects.Except((IList<TLI>)pi.Value)),
                "Id", "Name", emptyObject);

            sb.Append(html.DropDownList(String.Format("{0}{1}", pi.TagName, i), allList));

            sb.Append("</span>");

            if (!String.IsNullOrEmpty(pi.Description))
            {
                sb.AppendFormat("<div class=\"note\">{0}</div>", pi.Description);
            }

            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// Возвращает орисание элемента перечисления указанного в атрибуте DescriptionAttribute.
        /// </summary>
        /// <param name="type">Тип перечисления.</param>
        /// <param name="value">Элемент перечисления.</param>
        /// <returns>Описание элемента перечисления.</returns>
        public static string GetEnumItemDescription(Type type, object value)
        {
            FieldInfo fi = type.GetField(Enum.GetName(type, value));
            DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(
                fi, typeof(DescriptionAttribute));
            return da.Description;
        }

        private static string BBCodeNote =
            "<div class=\"note\">В этом поле можно использовать BBCode:</div>" +
            "<div class=\"note\">[b]<strong>Жирный</strong>[/b]</div>" +
            "<div class=\"note\">[i]<em>Курсив</em>[/i]</div>" +
            "<div class=\"note\">[u]<span style=\"text-decoration:underline\">Подчеркнутый</span>[/u]</div>" +
            "<div class=\"note\">[del]<span style=\"text-decoration:line-through\">Зачеркнутый</span>[/del]</div>" +
            "<div class=\"note\">[color=Red]<span style=\"color:Red\">Красный</span>[/color]</div>" +
            "<div class=\"note\">[url]<span><a href=\"http://example.com/sample/page\">http://example.com/sample/page</a></span>[/url]</div>" +
            "<div class=\"note\">[url=http://example.com/sample/page]<span><a href=\"http://example.com/sample/page\">Пример</a></span>[/url]</div>" +
            "<div class=\"note\">[img]<span><a href=\"http://example.com/sample/page\">http://example.com/sample/page</a></span>[/img]</div>";
    }
}
