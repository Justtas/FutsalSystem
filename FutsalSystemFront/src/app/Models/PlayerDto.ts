export class PlayerDto {
    public Id: number;
    public FirstName: string;
    public LastName: string;
    public DateOfBirth: Date;
    public ImagePath: string;
    public YellowCardsCount: number;
    public RedCardsCount: number;
    public MatchesPlayed: number;
    public Goals: number;
    public OwnGoals: number;
    public Number: number;
    public TeamId: number;
    public TeamName: string;

    constructor(id: number, firstName: string, lastName: string, dateOfBirth: Date, imagePath: string, yellowCardsCount: number, redCardsCount: number, matchesPlayed: number, goals: number, ownGoals: number, number: number, teamId: number, teamName: string) {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.DateOfBirth = dateOfBirth;
        this.ImagePath = imagePath;
        this.YellowCardsCount = yellowCardsCount;
        this.RedCardsCount = redCardsCount;
        this.MatchesPlayed = matchesPlayed;
        this.Goals = goals;
        this.OwnGoals = ownGoals;
        this.Number = number;
        this.TeamId = teamId;
        this.TeamName = teamName;
    }
}