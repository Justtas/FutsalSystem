import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { PlayerService } from '../Services/player.service';
import { PlayerDto } from '../Models/PlayerDto';
import { ActivatedRoute } from "@angular/router";
import { TeamService } from '../Services/team.service';
import { DeleteTeamModalComponent } from '../Modals/Team/delete-team-modal/delete-team-modal.component';
import { EditTeamModalComponent } from '../Modals/Team/edit-team-modal/edit-team-modal.component';
import { HubService } from '../Services/hub.service';


@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})
export class TeamComponent implements OnInit {

  public notLoaded = true;
  public dataSource: PlayerDto[] = [];
  public team: any = {};
  public teamid: string;
  public teamName: string;
  public isAdmin = false;

  constructor(private playerService: PlayerService, private teamService: TeamService, private router: Router, private dialog: MatDialog, private activatedRoute: ActivatedRoute, private signalRService: HubService) { }

  ngOnInit(): void {
    if (localStorage.getItem("userId") !== null) {
      this.isAdmin = true;
    }

    this.signalRService.teamUpdateSignal.subscribe((signal: any) => {
      this.onTeamUpdated(signal);
    });

    this.teamid = this.activatedRoute.snapshot.paramMap.get("id");

    this.teamService.getTeam('http://localhost:53078/api/teams/' + this.teamid).subscribe(item => {
      this.team = item;
      if (this.team.id === undefined)
      {
        this.router.navigate(['/teams']);
      }
      else {
        this.notLoaded = false;
        this.playerService.getPlayers('http://localhost:53078/api/players/byteam/' + this.teamid).subscribe(players => {
          this.dataSource = players;
        });
      }
    });
  }

  onPlayerRowClick(player: any) {
    this.router.navigate(['/players', player.id]);
  }

  openDeleteConfirmationDialog(): void {
    const dialogRef = this.dialog.open(DeleteTeamModalComponent, {
      width: '350px',
      data: "Ar tikrai norite ištrinti šią komandą?"
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.teamService.deleteTeam('http://localhost:53078/api/teams/' + this.teamid).subscribe(item => { this.router.navigate(['teams']); });
      }
    });
  }

  openTeamEditDialog(team: any) {
    console.log(team);
    let dialogRef = this.dialog.open(EditTeamModalComponent, { data: team });

    dialogRef.afterClosed().subscribe(result => {
      console.log('Dialog result:' + result);
    })
  }

  onTeamUpdated(signal) {
    console.log(signal);
    this.team.title = signal.title;
    this.team.imagePath = signal.imagePath;
  }
}
