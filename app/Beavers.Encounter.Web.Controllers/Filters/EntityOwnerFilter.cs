using System;
using System.Web.Mvc;
using Beavers.Encounter.Core;
using SharpArch.Core;
using SharpArch.Core.DomainModel;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    /// <summary>
    /// Фильтр проверяет права на доступ к объекту класса TEntity в контроллере TController.
    /// Предпологается, что во всех методах контроллера присутствует параметр
    /// с именем id илбо с именем сущности (Post запросы), которые четко определяют объект.
    /// Далее методом GetGameId определяется к какой игре принадлежит объект 
    /// и если объект принадлежит игре текущего автора (пользователя запросившего доступ), 
    /// то доступ предоставляется.
    /// </summary>
    /// <typeparam name="TEntity">Класс сущности.</typeparam>
    /// <typeparam name="TController">Контроллер сущности.</typeparam>
    public abstract class EntityOwnerFilter<TEntity, TController> : IActionFilter
        where TEntity : Entity
        where TController : BaseController
    {
        private readonly IRepository<TEntity> repository;

        protected EntityOwnerFilter(IRepository<TEntity> repository)
        {
            Check.Require(repository != null, "repository may not be null");

            this.repository = repository;
        }

        /// <summary>
        /// Определяет какой игре принадлежит указанный объект entity,
        /// т.е. возвращает id игры.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract int GetGameId(TEntity entity);

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.GetType() == typeof(TController))
            {
                // Пытаемся получить id объекта из строки запроса
                int entityId = -1;
                if (filterContext.ActionParameters.ContainsKey("id"))
                {
                    entityId = Convert.ToInt32(filterContext.ActionParameters["id"]);
                }

                // Пытаемся получить id объекта из формы
                if (filterContext.ActionParameters.ContainsKey(typeof(TEntity).Name.ToLower()))
                {
                    entityId = ((TEntity)filterContext.ActionParameters[typeof(TEntity).Name.ToLower()]).Id;
                }

                TEntity entity = repository.Get(entityId);
                if (entity != null && ((User)filterContext.HttpContext.User).Game.Id != GetGameId(entity))
                {
                    ((TController) filterContext.Controller).Message = "У вас нет прав на доступ в объекту.";
                    Uri urlReferrer = filterContext.HttpContext.Request.UrlReferrer;
                    filterContext.Result = new RedirectResult(urlReferrer == null ? "/Home" : urlReferrer.PathAndQuery);
                }
            }
        }
    }
}
