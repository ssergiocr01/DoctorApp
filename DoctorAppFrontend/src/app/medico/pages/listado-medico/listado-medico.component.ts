import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Medico } from '../../interfaces/medico';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MedicoService } from '../../servicios/medico.service';
import { CompartidoService } from 'src/app/compartido/compartido.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalMedicoComponent } from '../../modales/modal-medico/modal-medico.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-listado-medico',
  templateUrl: './listado-medico.component.html',
  styleUrls: ['./listado-medico.component.css'],
})
export class ListadoMedicoComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = [
    'apellidos',
    'nombres',
    'telefono',
    'genero',
    'nombreEspecialidad',
    'estado',
    'acciones',
  ];

  dataInicial: Medico[] = [];
  dataSource = new MatTableDataSource(this.dataInicial);
  @ViewChild(MatPaginator) paginacionTabla!: MatPaginator;

  constructor(
    private _medicoSevicio: MedicoService,
    private _compartidoServicio: CompartidoService,
    private dialog: MatDialog
  ) {}

  obtenerMedicos() {
    this._medicoSevicio.lista().subscribe({
      next: (data) => {
        if (data.isExitoso) {
          this.dataSource = new MatTableDataSource(data.resultado);
          this.dataSource.paginator = this.paginacionTabla;
        } else {
          this._compartidoServicio.mostrarAlerta(
            'No se encontraron datos',
            'Advertencia!'
          );
        }
      },
      error: (e) => {
        this._compartidoServicio.mostrarAlerta(e.error.errores, 'Error');
      },
    });
  }

  nuevoMedico() {
    this.dialog
      .open(ModalMedicoComponent, { 
        disableClose: true, 
        width: '600px' 
      })
      .afterClosed()
      .subscribe((resultado) => {
        if (resultado === 'true') {
          this.obtenerMedicos();
        }
      });
  }

  editarMedico(medico: Medico) {
    this.dialog
      .open(ModalMedicoComponent, {
        disableClose: true,
        width: '600px',
        data: medico,
      })
      .afterClosed()
      .subscribe((resultado) => {
        if (resultado === 'true') {
          this.obtenerMedicos();
        }
      });
  }

  removerMedico(medico: Medico) {
    Swal.fire({
      title: 'Desea Eliminar Medico',
      text: medico.nombres + ' ' + medico.apellidos,
      icon: 'warning',
      confirmButtonColor: '#3085d6',
      confirmButtonText: 'Si, eliminar',
      showCancelButton: true,
      cancelButtonColor: '#d33',
      cancelButtonText: 'No',
    }).then((resultado) => {
      if (resultado.isConfirmed) {
        this._medicoSevicio.eliminar(medico.id).subscribe({
          next: (data) => {
            if (data.isExitoso) {
              this._compartidoServicio.mostrarAlerta(
                'El médico fue eliminado',
                'Completo!'
              );
              this.obtenerMedicos();
            } else {
              this._compartidoServicio.mostrarAlerta(
                'No se pudo eliminar el médico',
                'Error!'
              );
            }
          },
          error: (e) => {
            this._compartidoServicio.mostrarAlerta(e.error.errores, 'Error!');
          },
        });
      }
    });
  }

  aplicarFiltroListado(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  ngOnInit(): void {
    this.obtenerMedicos();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginacionTabla;
  }
}
