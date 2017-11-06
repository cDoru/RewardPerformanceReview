
export class EmployeeDto {

    constructor() {
        this.username = '';
        this.password = '';
        this.firstName = '';
        this.surname = '';
        this.jobTitle = '';
        this.isAdmin = false;
    }

    username: string;
    password: string;
    firstName: string;
    surname: string;
    jobTitle: string;
    isAdmin: boolean;
}
