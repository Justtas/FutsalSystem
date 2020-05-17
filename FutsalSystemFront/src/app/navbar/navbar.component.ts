import { Component, OnInit } from '@angular/core';
import { $ } from 'protractor';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  public isAdmin = false;
  constructor(private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem("userId") !== null)
    {
      this.isAdmin = true;
      console.log(localStorage.getItem("userId"));
    }
  }

  toggle() {
    var mobileNav = document.getElementById('mobile_navigation');
    if(!mobileNav.classList.contains('active'))
      mobileNav.classList.add('active');
    else
      mobileNav.classList.remove('active');
  }

  logout() {
    localStorage.removeItem("userId");
    localStorage.removeItem("token");
    this.router.navigate(['home']);
  }
}
