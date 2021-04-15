using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace Tmall.Migration._100
{
    [Migration(2)]
    public class _0002_UserTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.User)
                .AutoId()
                .SmallIntForeignKeyIndexed("RoleId", Tables.Role, true, false)
                .WithColumn("Name").AsString(StringLength.Five).NotNullable()
                .Bool("IsTest", true)
                .IsActive();
        }
    }
}
