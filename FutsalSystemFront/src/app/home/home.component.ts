import { Component, OnInit } from '@angular/core';
import { TeamDto } from '../Models/TeamDto';
import { AnnouncementDto } from '../Models/AnnouncementDto';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AnnouncementService } from '../Services/announcement.service';
import { TeamService } from '../Services/team.service';
import { CreateAnnouncementModalComponent } from '../Modals/Announcement/create-announcement-modal/create-announcement-modal.component';
import { DeleteAnnouncementModalComponent } from '../Modals/Announcement/delete-announcement-modal/delete-announcement-modal.component';
import { EditAnnouncementModalComponent } from '../Modals/Announcement/edit-announcement-modal/edit-announcement-modal.component';
import { HubService } from '../Services/hub.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public announcements: any[] = [];
  public teams: TeamDto[] = [];
  public notLoaded = true;
  public isAdmin = false;

  constructor(private announcementService: AnnouncementService, private teamService: TeamService, private router: Router, public dialog: MatDialog, private signalRService: HubService) {}

  ngOnInit() {
    if (localStorage.getItem("userId") !== null) {
      this.isAdmin = true;
    }
    this.signalRService.announcementCreationSignal.subscribe((signal: any) => {
      this.onAnnouncementCreated(signal);
    });
    this.announcementService.getAnnouncements('http://localhost:53078/api/announcements/').subscribe(items => {
      this.announcements = items;
      this.teamService.getTeams('http://localhost:53078/api/teams/sorted').subscribe(tempTeams => { this.teams = tempTeams; });
      this.notLoaded = false;
    });
  }

  openAnnouncementCreateDialog() {
    let dialogRef = this.dialog.open(CreateAnnouncementModalComponent);

    dialogRef.afterClosed().subscribe(result => {
      let temp = [];
      this.announcementService.getAnnouncements('http://localhost:53078/api/announcements/').subscribe(items => {
        temp = items;
        if (temp.length != this.announcements.length)
          this.announcements.push(temp[temp.length - 1]);
      });
    });
  }

  openAnnouncementDeleteDialog(item, index, event): void {
    event.stopPropagation();
    const dialogRef = this.dialog.open(DeleteAnnouncementModalComponent, {
      width: '350px',
      data: "Ar tikrai norite ištrinti šią naujieną?"
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        let temp = [];
        console.log('Yes clicked');
        this.announcementService.deleteAnnouncement('http://localhost:53078/api/announcements/' + item.id).subscribe(item => {
          this.announcements.splice(index, 1);
        });
      }
    });
  }

  openAnnouncementEditDialog(item: any, event) {
    event.stopPropagation();
    console.log(item);
    let dialogRef = this.dialog.open(EditAnnouncementModalComponent, { data: item });

    dialogRef.afterClosed().subscribe(result => {
      console.log('Dialog result:' + result);
    })
  }

  onAnnouncementCreated(signal) {
    console.log(signal);
    this.announcements.unshift(signal); // pushina i pirma array vieta
  }
}

