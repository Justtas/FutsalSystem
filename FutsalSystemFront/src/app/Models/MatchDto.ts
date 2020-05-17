import { MatchEventDto } from './MatchEventDto';

export class MatchDto {
    public Id: number;
    public HomeTeamId: number;
    public HomeTeam: string;
    //public HomeTeam: TeamDto;

    public AwayTeamId: number;
    public AwayTeam: string;
    //public AwayTeam: TeamDto;

    public MatchDate: Date;
    public HomeTeamFirstHalfScore: number;
    public AwayTeamFirstHalfScore: number;
    public HomeTeamScore: number;
    public AwayTeamScore: number;
    public IsFinished: boolean;
    public MatchEvents: MatchEventDto[];

    constructor(id: number, homeTeamId: number, awayTeamId: number, homeTeam: string, awayTeam: string, matchDate: Date, homeTeamFirstHalfScore: number, awayTeamFirstHalfScore: number, homeTeamScore: number, awayTeamScore: number, isFinished: boolean, matchEvents: MatchEventDto[]) {
        this.Id = id;
        this.HomeTeamId = homeTeamId;
        this.AwayTeamId = awayTeamId;
        this.HomeTeam = homeTeam;
        this.AwayTeam = awayTeam;
        this.MatchDate = matchDate;
        this.HomeTeamFirstHalfScore = homeTeamFirstHalfScore;
        this.AwayTeamFirstHalfScore = awayTeamFirstHalfScore;
        this.HomeTeamScore = homeTeamScore;
        this.AwayTeamScore = awayTeamScore;
        this.IsFinished = isFinished;
        this.MatchEvents = matchEvents;
    }
}