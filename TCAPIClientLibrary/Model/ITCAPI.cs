#region License
/*
Copyright 2012 Rustici Software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion
using System;
using System.Collections.Generic;

using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public interface ITCAPI
    {
        void StoreStatement(Statement statement);
        void StoreStatements(Statement[] statements);
        void VoidStatements(string[] statementIdsToVoid);
        void StoreStatement(Statement statement, bool synchronous);
        void StoreStatements(Statement[] statements, bool synchronous);
        void VoidStatements(string[] statementIdsToVoid, bool synchronous);
        void Flush();

        Statement GetStatement(string statementId);
        StatementResult GetStatements(StatementQueryObject queryObject);
        StatementResult GetStatements(string moreUrl);

        ActivityProfile GetActivityProfile(string activityId, string profileId);
        void SaveActivityProfile(ActivityProfile profile, bool overwrite, ActivityProfile previous);
        void DeleteActivityProfile(string activityId, string profileId);
        void DeleteAllActivityProfile(string activityId);
        string[] GetActivityProfileIds(string activityId, NullableDateTime since);
        Activity GetActivity(string activityId);

        ActivityState GetActivityState(string activityId, Actor actor, string stateId, string registrationId);
        void SaveActivityState(ActivityState activityState, bool overwrite, ActivityState previousState);
        void DeleteActivityState(string activityId, Actor actor, string stateId, string registrationId);
        string[] GetActivityStateIds(string activityId, Actor actor, string registrationId, NullableDateTime since);

        ActorProfile GetActorProfile(Actor actor, string profileId);
        void SaveActorProfile(ActorProfile actorProfile, ActorProfile previousProfile, bool overwrite);
        void DeleteActorProfile(Actor actor, string profileId);
        void DeleteAllActorProfile(Actor actor);
        string[] GetActorProfileIds(Actor actor, NullableDateTime since);
        Actor GetActor(Actor partialActor);

        string GetOAuthAuthorizationUrl(string redirectUrl);
        OAuthAuthentication UpdateOAuthTokenCredentials(string temporaryCredentialsId, string verifierCode);
    }
}
