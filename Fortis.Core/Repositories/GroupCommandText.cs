namespace Fortis.Core.Repositories
{
    using System;

    public class GroupCommandText : IGroupCommandText
    {
        public string GetGroups => "Select group_id id, * from groups";
        public string GetGroupByID => "Select group_id id, * from groups where group_id = @group_id";
        public string GetGroupByName => "Select group_id id, * from groups where name = @name";
        public string InsertGroups => "insert into groups (group_id,name,group_type,adi_guid,member_count,current_risk_score,status) values (@group_id,@name,@group_type,@adi_guid,@member_count,@current_risk_score,@status);";
        public string UpdateGroups => "update groups set name = @name,group_type = @group_type,adi_guid = @adi_guid,member_count = @member_count,current_risk_score = @current_risk_score,status = @status where group_id = @id;";
    }
}