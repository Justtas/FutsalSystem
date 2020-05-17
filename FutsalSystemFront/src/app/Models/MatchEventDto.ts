import { PlayerEvent } from '../Enums/PlayerEvent';

export class MatchEventDto {
    public Id: number;
    public MatchId: number;
    public Minute: number;
    public PlayerId: number;
    public PlayerName: string;
    public TeamName: string;
    public EventType: PlayerEvent;

    constructor(id: number, matchId: number, minute: number, playerId: number, playerName: string, teamName: string, eventType: PlayerEvent) {
        this.Id = id;
        this.MatchId = matchId;
        this.Minute = minute;
        this.PlayerId = playerId;
        this.PlayerName = playerName;
        this.TeamName = teamName;
        this.EventType = eventType;
    }
}