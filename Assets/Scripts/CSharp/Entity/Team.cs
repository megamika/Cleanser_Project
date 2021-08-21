using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New team", menuName = "Team")]
public class Team : ScriptableObject
{
    public TeamRelationship[] relationships;
    Dictionary<Team, TeamRelationship> indexedRelationships;

    public TeamRelationship findRelationshipWith(Team team)
    {
        if (indexedRelationships == null) IndexTheRelationships();
        if (!indexedRelationships.ContainsKey(team)) return null;
        return indexedRelationships[team];
    }

    void IndexTheRelationships()
    {
        Debug.Log("Indexed the relationships for the team " + name);
        indexedRelationships = new Dictionary<Team, TeamRelationship>();
        foreach (var relationship in relationships)
        {
            indexedRelationships.Add(relationship.team, relationship);
        }
    }

    [System.Serializable]
    public class TeamRelationship
    {
        public string name;
        public bool canDamage;
        public Team team;
        public Relationship relationship;

        public enum Relationship
        {
            Ignore,
            Attack,
            Friendly
        }
    }
}
