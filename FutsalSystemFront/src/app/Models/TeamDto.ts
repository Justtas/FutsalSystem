import { MatchDto } from './MatchDto';
import { PlayerDto } from './PlayerDto';

export class TeamDto {
    public Id: number;
    public Title: string;
    public Points: number;
    public GoalsFor: number;
    public GoalsAgainst: number;
    public MatchesPlayed: number;
    public MatchesWon: number;
    public MatchesDrawn: number;
    public MatchesLost: number;
    public ImagePath: string;
    public HomeMatches: MatchDto[];
    public AwayMatches: MatchDto[];
    public Players: PlayerDto[];
    public GoalsDifference: number;

    constructor(id: number, title: string, points: number, goalsFor: number, goalsAgainst: number, matchesPlayed: number, matchesWon: number, matchesDrawn: number, matchesLost: number,
        imagePath: string, homeMatches: MatchDto[], awayMatches: MatchDto[], players: PlayerDto[], goalsDifference: number) {
        this.Id = id;
        this.Title = title;
        this.Points = points;
        this.GoalsFor = goalsFor;
        this.GoalsAgainst = goalsAgainst;
        this.MatchesPlayed = matchesPlayed;
        this.MatchesWon = matchesWon;
        this.MatchesDrawn = matchesDrawn;
        this.MatchesLost = matchesLost;
        this.ImagePath = imagePath;
        this.HomeMatches = homeMatches;
        this.AwayMatches = awayMatches;
        this.Players = players;
        this.GoalsDifference = goalsDifference;
    }
}