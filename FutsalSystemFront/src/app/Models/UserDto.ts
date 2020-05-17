export class UserDto {
    public Id: number;
    public Username: string;
    public Password: string;
    public Email: string;

    constructor(id: number, username: string, password: string, email: string) {
        this.Id = id;
        this.Username = username;
        this.Password = password;
        this.Email = email;
    }
}