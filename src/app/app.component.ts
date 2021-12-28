import { style } from '@angular/animations';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'AngularUI';
  openForm(){
    let formDisplay = document.getElementById("myForm");
    if (formDisplay!.style.display == '') {
      formDisplay!.style.display = "block";
    }
    else {
      formDisplay!.style.display = '';
    }
  }
}
