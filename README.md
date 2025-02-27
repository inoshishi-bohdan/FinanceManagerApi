# Finance Manager Api

A RESTful API designed to streamline income and expense tracking, as well as provide insightful financial statistics. Built using ASP.NET Core, this API leverages Entity Framework Core for efficient database interactions with SQL Server. 

## Features

- Comprehensive endpoints for managing incomes and expenses.
- Generation of financial statistics for analysis.
- Secure authentication and authorization using JSON Web Tokens (JWT).

## Installation

To set up and run the API, follow these steps:

1.  **Configure Database Connection:**
    * Open the `appsettings.json` file in the project.
    * Locate the `"FinanceManagerConnectionString"` setting.
    * Replace `<your local server name>` with the name of your local SQL Server instance.

    ```json
    "ConnectionStrings": {
      "FinanceManagerConnectionString": "Server=<your local server name>;Database=FinanceManager;Trusted_Connection=true;TrustServerCertificate=True"
    }
    ```

2.  **Run Database Migrations:**
    * Open the Package Manager Console in Visual Studio.
    * Execute the following command to apply Entity Framework Core migrations and create the database schema:

    ```powershell
    Update-Database
    ```

## Endpoints

### Authentication

*   **`POST /api/Auth/register`** - Register a new user.
    *   Request body: `RegisterRequestDto`
    *   Response: `UserDto`
*   **`POST /api/Auth/login`** - Log in and receive JWT tokens.
    *   Request body: `LoginRequestDto`
    *   Response: `TokenResponseDto`
*   **`POST /api/Auth/refreshToken`** - Refresh JWT tokens.
    *   Request body: `RefreshTokenRequestDto`
    *   Response: `TokenResponseDto`

### Currency

*   **`GET /api/Currency/getList`** - Get a list of all currencies.
    *   Response: Array of `CurrencyDto`

### Expenses

*   **`GET /api/Expense/getMyExpenses`** - Get a list of the user's expenses.
    *   Response: Array of `ExpenseDto`
*   **`POST /api/Expense/create`** - Create a new expense.
    *   Request body: `CreateExpenseRequestDto`
    *   Response: `ExpenseDto`
*   **`PUT /api/Expense/update/{id}`** - Update an existing expense.
    *   Request body: `UpdateExpenseRequestDto`
    *   Response: `ExpenseDto`
*   **`DELETE /api/Expense/delete/{id}`** - Delete an expense.
    *   Response: Success message

### Expense Categories

*   **`GET /api/ExpenseCategory/getList`** - Get a list of all expense categories.
    *   Response: Array of `ExpenseCategoryDto`

### Incomes

*   **`GET /api/Income/getMyIncomes`** - Get a list of the user's incomes.
    *   Response: Array of `IncomeDto`
*   **`POST /api/Income/create`** - Create a new income.
    *   Request body: `CreateIncomeRequestDto`
    *   Response: `IncomeDto`
*   **`PUT /api/Income/update/{id}`** - Update an existing income.
    *   Request body: `UpdateIncomeRequestDto`
    *   Response: `IncomeDto`
*   **`DELETE /api/Income/delete/{id}`** - Delete an income.
    *   Response: Success message

### Income Categories

*   **`GET /api/IncomeCategory/getList`** - Get a list of all income categories.
    *   Response: Array of `IncomeCategoryDto`

### Profile Images

*   **`GET /api/ProfileImage/getList`** - Get a list of all profile images.
    *   Response: Array of `ProfileImageDto`
*   **`GET /api/ProfileImage/{id}`** - Get a specific profile image by ID.
    *   Response: `ProfileImageDto`

### Statistics

*   **`POST /api/Statistic/income`** - Get income statistics.
    *   Request body: `GetStatisticRequestDto`
    *   Response: Array of `StatisticItemDto`
*   **`GET /api/Statistic/getIncomeRecordPeriod`** - Get income record periods.
    *   Response: Array of years
*   **`POST /api/Statistic/expense`** - Get expense statistics.
    *   Request body: `GetStatisticRequestDto`
    *   Response: Array of `StatisticItemDto`
*   **`GET /api/Statistic/getExpenseRecordPeriod`** - Get expense record periods.
    *   Response: Array of years
*   **`POST /api/Statistic/netWorth`** - Get net worth statistics.
    *   Request body: `GetStatisticRequestDto`
    *   Response: Array of `StatisticItemDto`
*   **`POST /api/Statistic/incomeDistribution`** - Get income distribution.
    *   Request body: `GetDistributionRequestDto`
    *   Response: Array of `DistributionItemDto`
*   **`POST /api/Statistic/expenseDistribution`** - Get expense distribution.
    *   Request body: `GetDistributionRequestDto`
    *   Response: Array of `DistributionItemDto`

### User

*   **`GET /api/User/getMyInfo`** - Get the current user's information.
    *   Response: `UserDto`
*   **`PUT /api/User/updateMyInfo`** - Update the current user's information.
    *   Request body: `UpdateUserRequestDto`
    *   Response: `UserDto`
*   **`DELETE /api/User/deleteMyAccount`** - Delete the current user's account.
    *   Response: Success message

## Contributing
Contributions are welcome! Please fork the repository and create a pull request with your changes.

## Contact
For any questions or support, please open an issue or contact the repository owner.
