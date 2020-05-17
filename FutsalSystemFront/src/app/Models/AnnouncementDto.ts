export class AnnouncementDto {
    public Id: number;
    public Title: string;
    public CreationDate: string;
    public Message: string;

    constructor(id: number, title: string, creationDate: string, message: string) {
        this.Id = id;
        this.Title = title;
        this.CreationDate = creationDate;
        this.Message = message;
    }
}