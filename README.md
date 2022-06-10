
# Diia Challange Task

Diia Challange Task is my task for Diia.Challange back-end competition (ASP.NET Core REST API)

## How to run

Go to the folder where you want to save the project and open the Command line there (or press Win + R, enter cmd, and click run, with cd command move to wanted folder).

**_Notice, that you have to have Git installed on your PC_**

In console, print:

    git clone https://github.com/everscope/diia.challenge-task.git

After the project has been cloned, print this:

    cd diia.challenge-task/Diia.Challenge
    dotnet run

In the console, among logs, you will be able to find this text with the address:

    Microsoft.Hosting.Lifetime[14]
    Now listening on: https://localhost:7174
    info: Microsoft.Hosting.Lifetime[14]
    Now listening on: http://localhost:5174
    info: Microsoft.Hosting.Lifetime[0]

Open your browser and go to the first address + "/swagger" (in this case `localhost:7079/swagger`) or second address + "/swagger" (`localhost:5079/swagger`). (notice, that in your case your ports can be different).

Instead of using swagger, you can use any software you like to send requests to the addresses above (Postman, for example)

When you finish, go back to the console and press Ctrl+C to stop the application.

  

## Task overview:

 - Web API works with a JSON file that contains the list of Cities, City districts, and Streets with the following structure:

	 **Cities**:

	    [
	        {
	    	    "id": "string"
	        }
	    ]

	**City districts**:

	    [
		    {
			    "id": "string",
			    "parentId": "cityId"
		    }
	    ]

	**Streets**:

	    [
		    {
			    "id": "string",
			    "parentId": "cityId",
			    "cityDistrictId": "string"
		    }
	    ]

	*where city `cityDistrictId` is optional

- API has to contain a request for the creation of new applications:

	    POST api/application
    	request:
    	{
    	"address": {
	    	"cityId": "string",
	    	"cityDistrictId": "string",
	    	"streetId": "string"
	    	}
    	}
    	response:
    	{
	    	"applicationId": "string"
	    }
    
 - API has to contain a request for making an update of the application:
    
		POST API/application/:id
		request:
		{
		"status": "string"
		}

- Applications have to be stored in a database.

 - Also, there are two files:

	 - one of them stores a threshold, which is set by request

		

		    POST api/threshold
		    request:
			{		    
				"value": 5
		    }

	 - another contains weights, which are set by request

	

		    POST api/weights
	    	request:
	    	{
		    	"{status}": 10,
		    	"{status2}": 1,
		    	...
	    	}

  

 - Weights and threshold are used while creating a new application to calculate weighted usage of address and based on this evaluation add a new address to the JSON file or not

  

### How to use:

You can test web API with swagger, but also you can send the next requests:

  

## Used technologies

-   ASP.NET Core
-   Entity Framework
-   XUnit
-   AutoMapper
-   Moq

