import { Component, OnInit, Inject } from '@angular/core';
import { TeamDto } from 'src/app/Models/TeamDto';
import { TeamService } from 'src/app/Services/team.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatchDto } from 'src/app/Models/MatchDto';
import { FormGroup, FormControl } from '@angular/forms';
import { MatchService } from 'src/app/Services/match.service';

@Component({
  selector: 'app-create-match-modal',
  templateUrl: './create-match-modal.component.html',
  styleUrls: ['./create-match-modal.component.scss']
})
export class CreateMatchModalComponent implements OnInit {

  public match: MatchDto;
  public selectedHomeTeamId: number;
  public selectedAwayTeamId: number;
  public selectedHomeTeamTitle: string;
  public selectedAwayTeamTitle: string;
  public teams: TeamDto[] = [];

  bioSection = new FormGroup({
    MatchDate: new FormGroup({
      matchDate: new FormControl('')
    })
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private teamService: TeamService, private matchService: MatchService, private router: Router, private snackBar: MatSnackBar, public dialogRef: MatDialogRef<CreateMatchModalComponent>) { }

  ngOnInit(): void {
    this.teamService.getTeams('http://localhost:53078/api/teams/').subscribe(items => { this.teams = items; console.log(this.teams); });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  updateMatchData() {
    if (this.validated())
    {
      this.match = new MatchDto(0, this.selectedHomeTeamId, this.selectedAwayTeamId, this.selectedHomeTeamTitle, this.selectedAwayTeamTitle, this.bioSection.value.MatchDate.matchDate, 0, 0, 0, 0, false, []);
      console.log(this.match);
      this.matchService.postMatch('http://localhost:53078/api/matches/', this.match)
        .subscribe(anything => { console.log(anything); });
        this.onNoClick();
    } else {
      this.snackBar.open("Duomenų laukai užpildyti neteisingai.", "Gerai", {
        duration: 4000,
      });
  }
}

  validated(): boolean {
    let dateTime = new Date()
    dateTime.setHours(dateTime.getHours() + 3);
    let timeNow = dateTime.toISOString();
    if (
      this.bioSection.value.MatchDate.matchDate < timeNow ||
      this.selectedAwayTeamId == undefined ||
      this.selectedHomeTeamId == undefined || this.selectedAwayTeamId == this.selectedHomeTeamId) {
      return false;
    }
    return true;
  }
}
