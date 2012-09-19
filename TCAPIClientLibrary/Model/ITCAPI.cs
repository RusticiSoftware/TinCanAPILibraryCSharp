using System;
using System.Collections.Generic;

using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public interface ITCAPI
    {
        void StoreStatement(Statement statement);
        void StoreStatements(Statement[] statements);
        void VoidStatements(String[] statementIdsToVoid);
        void StoreStatement(Statement statement, bool synchronous);
        void StoreStatements(Statement[] statements, bool synchronous);
        void VoidStatements(String[] statementIdsToVoid, bool synchronous);
        void Flush();

        Statement GetStatement(String statementId);
        StatementResult GetStatements(StatementQueryObject queryObject);
        StatementResult GetStatements(String moreUrl);

        ActivityProfile GetActivityProfile(string activityId, string profileId);
        void SaveActivityProfile(ActivityProfile profile, bool overwrite, ActivityProfile previous);
        void DeleteActivityProfile(string activityId, string profileId);
        void DeleteAllActivityProfile(string activityId);
        string[] GetActivityProfileIds(string activityId, NullableDateTime since);
        Activity GetActivity(string activityId);

        ActivityState GetActivityState(String activityId, Actor actor, String stateId, String registrationId);
        void SaveActivityState(ActivityState activityState, bool overwrite, ActivityState previousState);
        void DeleteActivityState(String activityId, Actor actor, String stateId, String registrationId);
        String[] GetActivityStateIds(String activityId, Actor actor, String registrationId, NullableDateTime since);

        ActorProfile GetActorProfile(Actor actor, String profileId);
        void SaveActorProfile(ActorProfile actorProfile, ActorProfile previousProfile, bool overwrite);
        void DeleteActorProfile(Actor actor, String profileId);
        void DeleteAllActorProfile(Actor actor);
        String[] GetActorProfileIds(Actor actor, NullableDateTime since);
        Actor GetActor(Actor partialActor);

        String GetOAuthAuthorizationUrl(String redirectUrl);
        OAuthAuthentication UpdateOAuthTokenCredentials(String temporaryCredentialsId, String verifierCode);
    }
}
