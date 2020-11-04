# AcousticConnections
Library used to connect to Acoustic XML api (previously IBMs WCA/Silverpop)

Make sure to check out the sample application which uses this library to make a simple API call to Acoustic service.
https://github.com/bak3r/AcousticConnectionsSample

A few mandatory tidbits on why so many configurable parameters. In the company I work for
we're have multiple Acoustic organizations (for marketing purposes) and one for transact. 
This is the reason there is an OrganizationId parameter in the IAcousticConfiguration interface.

The second thing is we have multiple applications and integrations using all those Acoustic organizations
and they are doing various tasks and creating jobs and such. So this is the reason there is an ApplicationId
parameter in the IAcousticConfiguration interface.

These two parameters are used for storage and retrieval of AccessToken

After importing library:

1. Make sure you implement classes that are needed to instantiate AcousticRestService.
Interfaces that define the classes to be implemented are:
- IAcousticConfiguration
- IAcousticLogger
- IAcousticStorage
- IJsonConverter

2. Instantiate AcousticRestService

3. Call PostAndReturnResponse method on AcousticRestService object

