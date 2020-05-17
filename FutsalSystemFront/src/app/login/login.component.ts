import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { UserDto } from '../Models/UserDto';
import { UserService } from '../Services/user.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  user: UserDto;
  token: any;
  userId: any;
  incorrect = false;

  bioSection = new FormGroup({
    Username: new FormGroup({
      username: new FormControl('')
    }),
    Password: new FormGroup({
      password: new FormControl('')
    })
  });

  constructor(private userService: UserService, private router: Router, private snackBar: MatSnackBar) { }

  ngOnInit() {
    if (localStorage.getItem("userId") !== null) {
      this.router.navigate(['home']);
    }
  }

  login() {
    this.user = new UserDto(0, this.bioSection.value.Username.username, this.bioSection.value.Password.password, '');

    if (this.validated()) {
      this.userService.postUser('http://localhost:53078/api/token', this.user).subscribe(anything => {
        if (anything.length === 0) {
          this.incorrect = true;
          this.snackBar.open("Administratoriaus vardas arba slaptažodis buvo įvestas neteisingai!", "Gerai", {
            duration: 4000,
          });
        } else {
          console.log(anything)
          localStorage.setItem('userId', anything.tempId);
          localStorage.setItem('token', anything.tokenString);
          this.snackBar.open("Sėkmingai prisijungėte!", "Gerai", {
            duration: 4000,
          });

          this.router.navigate(['home']);
        }
      });
    }
  }

  validated(): boolean {
    if (this.bioSection.value.Username.username == undefined ||
      this.bioSection.value.Password.password == undefined) {
      return false;
    }
    return true;
  }

  openCreateSuccessfulBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

}
