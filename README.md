# ChatGPT Clone

This project aims to replicate the functionality of OpenAI's ChatGPT using a Blazor front end and an ASP.NET Core backend. The application will allow users to interact with a conversational AI model through a clean and intuitive interface.

## Project Status

We're mid-development, but the API layer is now fully operational and ready for community testing! It includes three endpoints:
- **Start Conversation**: Initiates a new chat session.
- **Continue Conversation**: Allows continued interaction within an existing session.
- **Get All Conversations**: Retrieves a list of all ongoing and past conversations.

Feel free to test these functionalities and provide feedback!  I will be adding more comments and cleaning up commented code at a later date.  


## Requirements

- **Tier 0 (MVP)**: Basic chat functionality using OpenAI's API to start and maintain conversations.
- **Tier 1 (MVP+)**: Adds user authentication and conversation segregation based on user accounts.
- **Tier 2 (Final App)**: Integrates subscription management, payment processing, and other advanced features.

## Project Structure

The application is divided into several key components:

### ASP.NET Core Web API
- Serves as the backend server which handles API requests to and from the OpenAI API.
- Manages session data and user requests, ensuring that conversations can be started and maintained effectively.

### Blazor Front End
- A Blazor application that provides the user interface.
- Users can send messages and receive responses through this interface.
- Interacts with the ASP.NET Core Web API to process and display conversations.

### Database (Future Implementation)
- Intended for storing user information, conversation logs, and possibly subscription data.
- Will support advanced features like user authentication, history retrieval, and payment processing.

### Folder Structure

- `/ChatGPTCloneAPI` - Contains all backend API source code.
- `/ChatGPTCloneFrontEnd` - Contains all front-end source code using Blazor.
- `/ChatGPTCloneDatabase` - Placeholder for future database scripts and related database management code.

## Technology Stack

- **Backend**: ASP.NET Core Web API
- **Frontend**: Blazor (WebAssembly or Server, based on project needs)
- **Database**: SQL Server or another relational database management system (Future consideration)
- **Authentication**: ASP.NET Identity (Planned for future phases)

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

What you need to install the software:


dotnet sdk --version 6.0 or later
node.js --version 14.x or later
npm --version 6.x or later

### Installation

A step-by-step guide on setting up your development environment:

1. Clone the repository:

```
    bash

git clone https://github.com/markmahowald/ChatGPTCloneForEducation.git
```



### License

Distributed under the MIT License. See LICENSE for more information.

### Contact

Mark Mahowald- markmahowald0328@gmail.com


### Acknowledgments
[OpenAI](https://openai.com/)
[ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet)
[Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
Special thanks to the OpenAI Assistant(through chat GPT and the open ai API directly), which provided valuable input during the design and troubleshooting phases of the API layer.

