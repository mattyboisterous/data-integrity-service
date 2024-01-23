
# Data Integrity Service
This is a work in progress! !!
## Overview
This system is designed to manage, track and synchronise data changes between a mobile application and a cloud server. Designed to support a strict "offline first" capability it employs a service-oriented architecture enabling modularity, extensibility, and maintainability.

Key functionalities include data change tracking (both local and cloud), support for reference data synchronisation, resiliant HTTP communication, local caching (your choice of providers, both SQL and Non-SQL), transient error backoff, poison message queue and workflow management.

## Key components
Looking to seriously decouple some big players eventuated in two key concepts that are central to this system: IDataService and IWorkflow implementations.

### IDataService
**TL;DR**
*A concrete implementation of IDataService knows how to move/manipulate data for a single business entity (a table in a relational database, for example) to and from a mobile device and the cloud. It knows not how or when it will be used.*

The IDataService interface is central to the system's ability to handle data efficiently and effectively. The services implementing this interface are designed to manage the acquisition and storage of data in both server environments and local devices. Here are key aspects of IDataService:

**Dual Data Management**: Implementations of IDataService are capable of interacting with remote servers to fetch data and also managing local storage. This dual capability is essential for ensuring data availability and redundancy.

**Synchronization**: These services often handle synchronization between the server and local storage, ensuring that data is consistent and up-to-date across different environments.

**Abstraction Layer**: IDataService provides an abstraction layer over the data sources. This means that the rest of the system doesn’t need to be aware of the underlying data storage mechanisms, promoting loose coupling and scalability.

**Flexibility and Extensibility**: The interface allows for various implementations that can cater to different types of data or storage strategies (e.g., cloud-based storage, local databases, etc.), making the system adaptable to changing requirements.

**Asynchronous Operations**: Given the potentially time-consuming nature of data operations, especially involving network interactions, IDataService implementations likely support asynchronous methods to enhance performance and responsiveness.

### IWorkflowService
**TL;DR**
*A concrete implementation of IWorkflowService understands a pattern of how an IDataService implementation should be used. It ensures such a pattern is repeatable, reusable and it's behaviour is consistant.*

The IWorkflowService interface is pivotal in defining and managing the patterns for moving data between devices and servers. It encapsulates the logic for various operational flows, thereby orchestrating complex processes. Key roles include:

**Workflow Orchestration**: Implementations of IWorkflowService manage the sequence of steps involved in a particular process, such as data synchronization, validation, or transformation tasks.

**Reusability and Standardization**: By defining workflows as reusable services, the system promotes standardization of processes. This approach ensures consistency in how tasks are executed and simplifies maintenance.

**Decoupling Process Logic**: IWorkflowService abstracts the process logic from the actual execution context, allowing for greater flexibility in how workflows are utilized across the system.

**Integration Point**: These services act as an integration point between different system components, coordinating actions between IDataService instances, user interfaces, and other system parts.

**Error Handling and Retry Mechanisms**: Workflow services can encapsulate error handling and retry logic, ensuring robust execution even in the face of transient failures, especially important in network-dependent processes.

**Interplay between IDataService and IWorkflowService**
The interaction between IDataService and IWorkflowService instances is a critical aspect of the system's design. While IDataService instances focus on the 'how' of data management (storage, retrieval, synchronization), IWorkflowService instances concentrate on the 'when' and 'what' of operational sequences (orchestrating tasks, defining process flows). Together, they create a comprehensive framework that not only manages data effectively but also ensures that data-related processes are carried out in an efficient, reliable, and consistent manner.

In summary, IDataService and IWorkflowService form the backbone of a system designed for robust data management and process orchestration, making it well-suited for environments where reliable data handling and efficient workflow management are paramount.

### Core Components

**Service Factories**: Includes ChangeTrackingServiceFactory, DataServiceFactory, WorkflowServiceFactory, etc. These factories are responsible for creating instances of various services, adhering to the factory design pattern to facilitate service instantiation and management.

**Interfaces**: A series of interfaces like IChangeTrackingService, IDataService, IHttpService, ILocalCacheService, etc., define contracts for the services, promoting a design that supports flexibility and interoperability among different service implementations.

**Mock Services**: Such as MockHttpChangeTrackingService, MockLocalChangeTrackingService, etc., are used for testing purposes. These mock implementations simulate real-world service behavior, crucial for unit testing and ensuring system reliability without external dependencies.

**Models**: Classes like MemoModel, ProvisionModel, QualityAreaModel, etc., represent the data structures used within the system. These models are essential for representing business entities and are manipulated by the services.

**Workflows**: Classes like DeleteInsertAllByKeyFlow, PullFromServerFlow, etc., handle specific business logic or data processing tasks, indicating the system's capability to manage complex operational flows.

### Key Features

**Change Tracking**: The system efficiently tracks data changes, allowing for a robust data integrity mechanism.

**Service-Oriented Architecture**: The modular design ensures each component is self-contained, facilitating easier maintenance and scalability.

**Extensibility**: The use of interfaces and factory patterns allows for easy integration of new service types and data models.

**Testing Framework**: Mock services provide a framework for thorough unit testing, critical for maintaining high-quality code.

**Workflow Management**: The system can handle complex workflows, demonstrating its capability to manage and automate various business processes.
Technical Aspects:

**Dependency Injection**: Used extensively for service creation, enhancing the flexibility and testability of the system.

**Asynchronous Operations**: Many service methods are asynchronous, indicating the system's capability to handle intensive operations efficiently.

**HTTP and Local Services**: The system interacts with remote services via HTTP and manages local data, indicating a comprehensive approach to data handling.

**Conclusion**:
This Data Integrity and Change Tracking System exemplifies a well-architected software solution that balances modularity with functionality. Its emphasis on service orientation, testing, and efficient data handling positions it as a robust and scalable platform for managing data integrity and workflows.

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
