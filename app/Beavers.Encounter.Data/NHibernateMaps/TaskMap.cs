using Beavers.Encounter.Core;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TaskMap : IAutoMappingOverride<Task>
    {
        public void Override(AutoMap<Task> mapping)
        {
            mapping.Map(x => x.TaskType).CustomTypeIs(typeof (TaskTypes));

            mapping.HasMany(x => x.Tips).Inverse().Cascade.Delete();
            mapping.HasMany(x => x.Codes).Inverse().Cascade.Delete();
            
            mapping.HasManyToMany(x => x.NotAfterTasks)
                .WithTableName("PreventPastTasks")
                .WithChildKeyColumn("TaskFk")
                .WithParentKeyColumn("TaskRefFk");
            
            mapping.HasManyToMany(x => x.NotOneTimeTasks)
                .WithTableName("PreventOneTimeTasks")
                .WithChildKeyColumn("TaskFk")
                .WithParentKeyColumn("TaskRefFk");

            mapping.HasManyToMany(x => x.NotForTeams)
                .WithTableName("PreventTeamTasks")
                .WithChildKeyColumn("TeamFk")
                .WithParentKeyColumn("TaskFk");
        }
    }
}
