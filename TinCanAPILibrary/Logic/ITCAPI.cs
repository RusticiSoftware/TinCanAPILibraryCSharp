using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Logic
{
    public interface ITCAPI
    {
        void StoreStatement(Statement statement);
        void StoreStatements(Statement[] statements);
        void VoidStatements(String[] statementIdsToVoid);
        void StoreStatement(Statement statement, bool overwrite);
        void StoreStatements(Statement[] statements, bool overwrite);
        void VoidStatements(String[] statementIdsToVoid, bool overwrite);
        void Flush();

        Statement GetStatement(String statementId);
        StatementResult GetStatements(StatementQueryObject queryObject);
        StatementResult GetStatements(String moreUrl);

        ActivityProfile GetActivityProfile(string activityId, string profileId);
        void SaveActivityProfile(ActivityProfile profile, bool overwrite = false, ActivityProfile previous = null);
        void DeleteActivityProfile(string activityId, string profileId);
        void DeleteAllActivityProfile(string activityId);
        string[] GetActivityProfileIds(string activityId, NullableDateTime since = null);
        Activity GetActivity(string activityId);

        ActivityState GetActivityState(String activityId, Actor actor, String stateId, String registrationId = null);
        void SaveActivityState(ActivityState activityState, bool overwrite = true, ActivityState previousState = null);
        void DeleteActivityState(String activityId, Actor actor, String stateId, String registrationId = null);
        String[] GetActivityStateIds(String activityId, Actor actor, String registrationId = null, NullableDateTime since = null);

        ActorProfile GetActorProfile(Actor actor, String profileId);
        void SaveActorProfile(ActorProfile actorProfile, ActorProfile previousProfile, bool overwrite = false);
        void DeleteActorProfile(Actor actor, String profileId);
        void DeleteAllActorProfile(Actor actor);
        String[] GetActorProfileIds(Actor actor, NullableDateTime since = null);
        Actor GetActor(Actor partialActor);

        String GetOAuthAuthorizationUrl(String redirectUrl = null);
        OAuthAuthentication UpdateOAuthTokenCredentials(String temporaryCredentialsId, String verifierCode);
    }
}
