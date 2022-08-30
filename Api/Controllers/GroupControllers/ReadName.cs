﻿using Api.Models.ExpositionModels.Resources;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("name"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<GroupResource>>> ReadName(string name)
    {
        /*
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest();
        }

        List<GroupResource_WithMembers> groups = new List<GroupResource_WithMembers>();

        await this.context.Groups
            .ForEachAsync(g =>
            {
                if (groups.Count >= this.MAX_GROUPS_IN_SEARCHES)
                {
                    return;
                }

                if (LevenshteinDistance.Compare(name, g.Name) >= 0.5)
                {
                    groups.Add(this.mapper.Group_ToResource_WithMembers(g));
                }
            });

        return Ok(groups);
        */
        return Ok();
    }
}
