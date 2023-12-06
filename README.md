## Immediate goals

### Ensure consistant capture of exceptions on the mobile device.
Exception capture is not as consistant as it should be in the Data Integrity Service. As a preliminary action we should ensure that all exceptions are captured and reported via the AppCenter frameworks 'Crashes.TrackError'. Information captured here is then routed to Azure Application Insights. 

The development team should identify the branch matching the existing production deployment, introduce the new exception capturing statements and deploy a new mobile application release. Ensuring only additional exception handling is added should ensure we can generate an update quickly and with a low level of risk.

### Monitor Application Insights.
Once the mobile application has been redeployed, Application Insights should be monitored closely with a focus on DIS metrics (crash details and service run duration, in particular)

### Identify/categorise data modelling on the mobile device.
While the development team is familiar with the Sql Azure R4Q database tables, there is a lot less visibility of the data modelling on the R4Q mobile application (some data is stored in a local relational DB and some is stored on a local key/object cache, for example). Data mapping between the R4Q mobile application and the R4Q Sql Azure database should be clarified.

## Intermediate goals

### The existing means to determine if data needs to be syncronised between mobile device and cloud is inefficient.
One of the existing design goals of the Data Integrity Service was that it should be able to run often, and only ever perform work if there is any to do. The means of determining if there are differences between mobile device and cloud are calculated in real time. This approach, while valid, is not performant at scale, particularly on the cloud/server side.

A good intermediate step would be a partial refactor to ensure these calculations are not performed in real time but are calculated when data changes, and then stored. Determining if data synchronisation is required would then be calculated with a single read, and not calculating a result in real time by pulling data from multiple tables. This approach should result in significant and scalable performance gains.

## Eventual goals

### Refactor the Data Integrity Service to ensure it is compatible with MAUI, modular and reusable.
The existing codebase for the Data Integrity Service has become convoluted, and difficult to maintain and understand. The impending migration to MAUI presents us with an opportunity to simplify and refactor the service, making it modular by design. This would ensure the service can be added to future mobile applications (including the MAUI version of R4Q) as a reusable component. Adding or modifying data service components responsible for synchronsing specific datasets would be simplified. Understanding how the service works would be a lot easier. Data transfer across all datasets would be performant, with all components monitored in a consistant fashion.
