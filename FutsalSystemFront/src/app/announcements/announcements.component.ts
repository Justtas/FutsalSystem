import { Component, OnInit } from '@angular/core';
import { AnnouncementDto } from '../Models/AnnouncementDto';
import { AnnouncementService } from '../Services/announcement.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-announcements',
  templateUrl: './announcements.component.html',
  styleUrls: ['./announcements.component.scss']
})
export class AnnouncementsComponent implements OnInit {

  public dataSource: AnnouncementDto[] = [];


  constructor(private announcementService: AnnouncementService, private router: Router, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.announcementService.getAnnouncements('http://localhost:53078/api/announcements/').subscribe(items => {this.dataSource = items; console.log(this.dataSource); });
  }

}
