import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CompartidoService } from '../compartido.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit {

  username:string = '';

  constructor(private router:Router, private compartidoService: CompartidoService) 
  {
  }

  ngOnInit(): void {
    const usuarioSesion = this.compartidoService.obtenerSesion();
    
    if(usuarioSesion!=null)
    {
      this.username = usuarioSesion;
    }
  }

  cerrarSesion(){
    this.compartidoService.eliminarSesion();
    this.router.navigate(['login']);
  }
}
