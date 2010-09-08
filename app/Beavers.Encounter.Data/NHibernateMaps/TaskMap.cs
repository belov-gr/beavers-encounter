using Beavers.Encounter.Core;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TaskMap : IAutoMappingOverride<Task>
    {
        public void Override(AutoMapping<Task> mapping)
        {
            mapping.Map(x => x.TaskType).CustomType(typeof (TaskTypes));
            mapping.Map(x => x.GiveTaskAfter).CustomType(typeof(GiveTaskAfter));

            mapping.HasMany(x => x.Tips).Inverse().Cascade.Delete();
            mapping.HasMany(x => x.Codes).Inverse().Cascade.Delete();
            
            mapping.HasManyToMany(x => x.NotAfterTasks)
                .Table("PreventPastTasks")
                .ChildKeyColumn("TaskFk")
                .ParentKeyColumn("TaskRefFk");
            
            mapping.HasManyToMany(x => x.NotOneTimeTasks)
                .Table("PreventOneTimeTasks")
                .ChildKeyColumn("TaskFk")
                .ParentKeyColumn("TaskRefFk");

            mapping.HasManyToMany(x => x.NotForTeams)
                .Table("PreventTeamTasks")
                .ChildKeyColumn("TeamFk")
                .ParentKeyColumn("TaskFk");
        }
    }
}
