import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-edituser',
  templateUrl: './edituser.component.html',
  styleUrls: ['./edituser.component.css']
})
export class EdituserComponent implements OnInit {
  constructor(private httpClient: HttpClient, private route: ActivatedRoute, 
    private formBuilder: FormBuilder, private router: Router) {
   }
  BaseUrl = 'https://localhost:44346/';
  userId : any;
  roles: any;
  user: any;
  files: any;
  link:any;
  totalLength:any;
  page:number=1;
  editForm=this.formBuilder.group({
    userName: ['', Validators.required],
    email: ['', Validators.email],
    role:''
  });
  ngOnInit(): void {
    this.getUser();
    this.getRoles();
    this.getFiles();
  }
  getUser(){
    this.route.queryParams
    .subscribe(params => {
      this.userId = params['userId']
    });
    this.httpClient.get(this.BaseUrl + "users/"+this.userId).subscribe(
      (res)=>this.user = res,
      (err)=>console.log(err)
    );
  }
  getRoles(){
    this.httpClient.get(this.BaseUrl + "roles").subscribe(
      (res)=> this.roles = res,
      (err)=> console.log(err)
    );
  }
   updateUser(){
     this.httpClient.put(this.BaseUrl + "users/" + this.userId + "/" + this.editForm.controls['role'].value, this.editForm.value).subscribe(
      (res)=> this.router.navigate(['/administration']),
      (err)=> console.log(err));
   }
   getFiles(){
     this.httpClient.get(this.BaseUrl + "files/" + this.userId).subscribe(
       (res)=>this.files = res,
       (err)=>console.log(err)
     )
   }
   deleteFile(file:any){
     this.httpClient.delete(this.BaseUrl + "files/" + file.id).subscribe(
       (res)=>this.getFiles(),
       (err)=>console.log(err)
     )
   }
   public uploadFile = (files:any) => {
    if (files.length === 0) {
      return;
    }
    const httpOptions = {
      headers: new HttpHeaders({
      'accept': 'application/json'
    })
    }
    let uploadedFile = <File>files[0];
    const formData = new FormData();
    formData.append('uploadedFile', uploadedFile, uploadedFile.name);
    this.httpClient.post(this.BaseUrl + "files/upload", formData, httpOptions).subscribe(
      ()=>this.getFiles(),
      (err)=>console.log(err)
    )
  }
  shareFile(file:any){
    let popup = document.getElementById("overlay")!;
    popup.style.position ="absolute";
    popup.style.opacity =".5";
    popup.style.zIndex ="998";
    popup.style.left ="0px";
    popup.style.width="100%";
    document.getElementById("popup")!.style.display = "block";  
    this.link = this.BaseUrl + "files/download/" + file.id;
  }
  close(){
    let popup = document.getElementById("overlay")!;
    popup.style.position ="";
    popup.style.opacity ="";
    popup.style.zIndex ="";
    popup.style.left ="";
    document.getElementById("popup")!.style.display = "";
  }
   get email(){
    return this.editForm.get('email');
  }
  get userName(){
    return this.editForm.get('userName');
  }
}
