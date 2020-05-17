import { Component, OnInit } from '@angular/core';
import { MatchService } from '../Services/match.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { TeamService } from '../Services/team.service';
import { TeamDto } from '../Models/TeamDto';
import { MatchDto } from '../Models/MatchDto';
import { CreateMatchModalComponent } from '../Modals/Match/create-match-modal/create-match-modal.component';
import { HubService } from '../Services/hub.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-matches',
  templateUrl: './matches.component.html',
  styleUrls: ['./matches.component.scss']
})
export class MatchesComponent implements OnInit {

  public dataSource: MatchDto[];
  public fullInfoCoppied: any[] = [];
  public searchMatch: any;
  public notLoaded = true;
  public isAdmin = false;

  constructor(private matchService: MatchService, private teamService: TeamService, private router: Router, private route: ActivatedRoute, public dialog: MatDialog, private signalRService: HubService, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    if (localStorage.getItem("userId") !== null) {
      this.isAdmin = true;
    }

    this.signalRService.matchCreationSignal.subscribe((signal: any) => {
      this.onMatchCreated(signal);
    });

    this.matchService.getMatches('http://localhost:53078/api/matches/').subscribe(items => {
      this.dataSource = items;
      this.notLoaded = false;
      this.assignCopy();
    });
  }

  onMatchRowClick(match: any) {
    this.router.navigate(['/matches/', match.id]);
  }

  openMatchCreateDialog() {
    let dialogRef = this.dialog.open(CreateMatchModalComponent);

    dialogRef.afterClosed().subscribe(result => {
    });
  }

  onMatchCreated(signal) {
    this.dataSource.push(signal);
    this.assignCopy();
    this.openCreateSuccessfulBar('Rungtynės buvo sėkmingai pridėtos!', 'Gerai');
  }

  openCreateSuccessfulBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

  assignCopy() {
    this.fullInfoCoppied = Object.assign([], this.dataSource);
  }

  filterItem(value) {
    if (!value) {
      this.assignCopy();
    } // when nothing has typed
    this.fullInfoCoppied = Object.assign([], this.dataSource).filter(
      item => item.homeTeam.toLowerCase().indexOf(value.toLowerCase()) > -1 ||
        item.awayTeam.toLowerCase().indexOf(value.toLowerCase()) > -1 ||
        item.matchDate.toString().toLowerCase().indexOf(value.toString().toLowerCase()) > -1
    )
  }
}
