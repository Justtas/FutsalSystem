import { Component, OnInit } from '@angular/core';
import { TeamDto } from '../Models/TeamDto';
import { TeamService } from '../Services/team.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { PlayerDto } from '../Models/PlayerDto';
import { PlayerService } from '../Services/player.service';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {

  public teams: TeamDto[] = [];
  public players: PlayerDto[] = [];
  public playersByYellowCards: PlayerDto[] = [];
  public playersByRedCards: PlayerDto[] = [];
  public playersByGoals: PlayerDto[];
  public matchId: number;
  public notLoaded = true;

  constructor(private teamService: TeamService, private playerService: PlayerService, private router: Router, public dialog: MatDialog) { }

  ngOnInit() {
    this.teamService.getTeams('http://localhost:53078/api/teams/sorted').subscribe(items => {
      this.teams = items;
      this.playerService.getPlayers('http://localhost:53078/api/players').subscribe(players => {
        this.players = players;
        this.playersByGoals = players.slice();
        this.playersByYellowCards = players.slice();
        this.playersByRedCards = players.slice();

        let tempByGoals = this.sortByGoals(this.playersByGoals);
        this.playersByGoals = tempByGoals.slice(0, 5);

        let tempByRedCards = this.sortByRedCards(this.playersByRedCards);
        this.playersByRedCards = tempByRedCards.slice(0, 5);

        let tempByYellowCards = this.sortByRedCards(this.playersByYellowCards);
        this.playersByYellowCards = tempByYellowCards.slice(0, 5);
      });
      this.notLoaded = false;
    });
  }

  sortByGoals(array: any[]): any[] {
    array = array.slice();

    array.sort((a, b) => (a.goals < b.goals) ? 1 : -1);

    return array;
  }

  sortByYellowCards(array: any[]): any[] {
    array = array.slice();

    array.sort((a, b) => (a.yellowCardsCount < b.yellowCardsCount) ? 1 : -1);

    return array;
  }

  sortByRedCards(array: any[]): any[] {
    array = array.slice();

    array.sort((a, b) => (a.redCardsCount < b.redCardsCount) ? 1 : -1);

    return array;
  }


  onTeamRowClick(team: any) {
    this.router.navigate(['/teams', team.id]);
  }

  onPlayerRowClick(player: any) {
    this.router.navigate(['/players', player.id]);
  }

}
