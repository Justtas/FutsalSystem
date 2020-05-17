import { Component, OnInit } from '@angular/core';
import { MatchService } from '../Services/match.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatchEventDto } from '../Models/MatchEventDto';
import { MatchEventService } from '../Services/matchEvent.service';
import { TeamService } from '../Services/team.service';
import { DeleteMatchModalComponent } from '../Modals/Match/delete-match-modal/delete-match-modal.component';
import { EditMatchModalComponent } from '../Modals/Match/edit-match-modal/edit-match-modal.component';
import { CreateMatchEventModalComponent } from '../Modals/Match/create-match-event-modal/create-match-event-modal.component';
import { HubService } from '../Services/hub.service';
import { DeleteMatchEventModalComponent } from '../Modals/Match/delete-match-event-modal/delete-match-event-modal.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-match',
  templateUrl: './match.component.html',
  styleUrls: ['./match.component.scss']
})
export class MatchComponent implements OnInit {

  public matchEventNames: string[] = ['Įvartis', 'Savas įvartis', '6m baudinys', '10m baudinys', 'Geltona kortelė', 'Raudona kortelė', 'Neįmuštas baudinys'];
  public match: any = {};
  public homeTeam: any = {};
  public awayTeam: any = {};
  //public matchEvents: MatchEventDto[] = [];
  public matchEvents: any[] = [];
  public matchId: number;
  public notLoaded = true;
  public isAdmin = false;
  public canBeAdded = false;

  constructor(private matchService: MatchService, private matchEventService: MatchEventService, private teamService: TeamService, private router: Router, private route: ActivatedRoute, public dialog: MatDialog, private signalRService: HubService, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    if (localStorage.getItem("userId") !== null) {
      this.isAdmin = true;
    }

    this.signalRService.matchEventCreationSignal.subscribe((signal: any) => {
      this.onMatchEventCreated(signal);
    });

    this.signalRService.matchEndedSignal.subscribe((signal: any) => {
      this.onMatchEnded(signal);
    });

    this.route.params.subscribe(params => {
      this.matchId = params['id'];
    });
    this.matchService.getMatch('http://localhost:53078/api/matches/' + this.matchId).subscribe(match => {
      this.match = match;
      if (this.match.id === undefined)
      {
        this.router.navigate(['/matches']);
      }
      else {
        this.matchEventService.getMatchEvents('http://localhost:53078/api/matches/' + this.matchId + '/matchEvents').subscribe(events => {
          this.matchEvents = events;
          console.log(this.matchEvents);
          this.teamService.getTeam('http://localhost:53078/api/teams/' + this.match.homeTeamId).subscribe(homeT => {
            this.homeTeam = homeT;
            this.match.homeTeam = this.homeTeam.title;
            this.teamService.getTeam('http://localhost:53078/api/teams/' + this.match.awayTeamId).subscribe(awayT => {
              this.awayTeam = awayT;
              this.match.awayTeam = this.awayTeam.title;
              this.notLoaded = false;
            });
          });
        });
      }
    });

  }

  openDeleteConfirmationDialog(): void {
    const dialogRef = this.dialog.open(DeleteMatchModalComponent, {
      width: '350px',
      data: "Ar tikrai norite ištrinti šias rungtynes?"
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.matchService.deleteMatch('http://localhost:53078/api/matches/' + this.matchId).subscribe(item => { this.router.navigate(['matches']); });
      }
    });
  }

  openMatchEditDialog(match: any) {
    let dialogRef = this.dialog.open(EditMatchModalComponent, { data: match });

    dialogRef.afterClosed().subscribe(result => {
    })
  }

  openMatchEventCreateDialog(match: any) {
    let dialogRef = this.dialog.open(CreateMatchEventModalComponent, { data: match });
    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      this.canBeAdded = true;

      this.matchEvents.forEach(element => {
        if (element.playerId === result.playerId)
        {
        // TIKRINAMA AR ZAIDEJAS PRIES SI IVYKI TURI RAUDONA KORTELE
          if (element.eventType === 5 && element.minute < result.minute)
          {
            this.canBeAdded = false;
            this.openErrorMessageBar("Žaidėjas yra gavęs raudoną kortelę, todėl įvykis negali būti pridėtas!", "Gerai");
          }
          // TIKRINAMA AR ZAIDEJUI TAI YRA ANTRA GELTONA KORTELE
          if (element.eventType === 4 && result.eventType === 4 && element.minute < result.minute)
          {
            result.eventType = 5;
          }
        }
      });
      console.log(this.canBeAdded);
      if (this.canBeAdded && result !== undefined)
      {
        this.matchEvents.push(result);
      }
      console.log(this.matchEvents);
    });
  }

  openErrorMessageBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 10000,
    });
  }

  onUpdateMatchEventsClick() {
    this.match.matchEvents = this.matchEvents;
    console.log(this.match.matchEvents);
    this.matchService.UpdateMatchWithMatchEvents('http://localhost:53078/api/matches/' + this.matchId + '/matchEventsUpdate', this.match).subscribe(x => {
    });
  }

  onMatchEventCreated(signal) {
    console.log(signal);
    this.matchEvents.push(signal);
  }

  onMatchEnded(signal) {
    this.match.isFinished = signal.isFinished;
    this.match.homeTeamScore = signal.homeTeamScore;
    this.match.awayTeamScore = signal.awayTeamScore;
  }

  openMatchEventDeleteDialog(element, i, $event) {
    event.stopPropagation();
    const dialogRef = this.dialog.open(DeleteMatchEventModalComponent, {
      width: '350px',
      data: "Ar tikrai norite ištrinti šį rungtynių įvykį?"
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(element);
      if (result) {
        this.matchEvents.splice(i, 1);
      }
    });
  }
}
