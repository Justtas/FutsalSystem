import { Component, OnInit } from '@angular/core';
import { TeamService } from '../Services/team.service';
import { TeamDto } from '../Models/TeamDto';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CreateTeamComponent } from '../Modals/Team/create-team-modal/create-team-modal.component';
import { HubService } from '../Services/hub.service';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.scss']
})
export class TeamsComponent implements OnInit {

  public dataSource: TeamDto[] = [];
  public notLoaded = true;
  public isAdmin = false;

  constructor(private teamService: TeamService, private router: Router, public dialog: MatDialog, private signalRService: HubService) { }

  ngOnInit() {
    if (localStorage.getItem("userId") !== null) {
      this.isAdmin = true;
    }
    
    this.signalRService.teamCreationSignal.subscribe((signal: any) => {
      this.onTeamCreated(signal);
    });

    this.teamService.getTeams('http://localhost:53078/api/teams/').subscribe(items => {
    this.dataSource = items; 
    this.notLoaded = false;
    });
  }

  onTeamsRowClick(team: any) {
    this.router.navigate(['/teams', team.id]);
  }

  openTeamCreateDialog() {
    let dialogRef = this.dialog.open(CreateTeamComponent);

    dialogRef.afterClosed().subscribe(result => {
      let temp = [];
      this.teamService.getTeams('http://localhost:53078/api/teams/').subscribe(items => {
        temp = items;
        if (temp.length != this.dataSource.length)
          this.dataSource.push(temp[temp.length - 1]);
      });
    });
  }

  onTeamCreated(signal) {
    this.dataSource.push(signal);
  }
}
