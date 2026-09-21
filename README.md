## Screenshots

<details>
<summary><strong>📸 View Application Photos</strong></summary>

### Contacts Manager

Main contacts page with contact information, CRUD actions, search, filtering, and export options.

![Contacts Manager](assets/contacts-manager-page.png)

### Search Options

Available fields for searching and filtering contacts.

![Search Options](assets/contacts-manager-page-options.png)

### Search List

Search field selection and search interface.

![Search List](assets/contacts-manager-page-searchlist.png)

### Filtered Contacts

Example of filtering contacts by country.

![Filtered Contacts](assets/contacts-manager-page-fltered.png)

### Export Contacts as PDF

Contacts exported to a PDF document.

![PDF Export](assets/contacts-manager-page-as-pdf.png)

### Export Contacts as Excel

Example of exported contact data in Excel.

![Excel Export](assets/contactsasexcel.png)

### User Login

Login page for authenticated users.

![Login](assets/login.png)

### Login Validation

Example of login validation when invalid credentials are submitted.

![Login Validation](assets/login-validations.png)

### User Registration

Registration form with User/Admin role selection.

![Register](assets/register.png)

### Registration Validation

Registration form validation for invalid or missing input.

![Registration Validation](assets/register_validation.png)

### Registration Validation Examples

Additional registration validation scenarios, including password and phone-number validation.

![Registration Validations](assets/register_validations.png)

### Asynchronous Registration Validation

Asynchronous AJAX validation during user registration.

![Asynchronous AJAX Validation](assets/register_asynchronous_ajax.png)

### Upload Contacts from Excel

Upload contacts from an Excel file directly through the application.

![Upload Contacts From Excel](assets/uploadcontactsfromexcel.png)

### Error Handling

Custom error page displayed when an application error occurs.

![Error Page](assets/errorpage.png)

</details>

## Database

The application uses **SQL Server** with **Entity Framework Core** for database operations.

The application supports typical CRUD operations:

```text
Create → Read → Update → Delete
```

## Security

The application includes several security measures:

- **HTTPS** – The application is configured to use HTTPS for secure communication.
- **CSRF Protection** – Anti-forgery protection is implemented to help prevent **Cross-Site Request Forgery (CSRF)** attacks.
- **Authentication & Authorization** – Protected pages require authentication, with support for role-based authorization.
- **Input Validation** – User input is validated on both the client and server sides.

## Authentication & Authorization

Authentication and authorization are implemented using **ASP.NET Core Identity**.

The application supports:

- User registration
- Login/logout
- Role-based authorization
- User and Admin roles
- Protected application pages
- Validation of registration and login data
