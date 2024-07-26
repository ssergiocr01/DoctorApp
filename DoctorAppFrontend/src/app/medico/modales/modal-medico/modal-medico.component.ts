import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Medico } from '../../interfaces/medico';
import { MedicoService } from '../../servicios/medico.service';
import { CompartidoService } from 'src/app/compartido/compartido.service';
import { Especialidad } from 'src/app/especialidad/interfaces/especialidad';
import { EspecialidadService } from 'src/app/especialidad/servicios/especialidad.service';

@Component({
  selector: 'app-modal-medico',
  templateUrl: './modal-medico.component.html',
  styleUrls: ['./modal-medico.component.css'],
})
export class ModalMedicoComponent implements OnInit {
  formMedico: FormGroup;
  titulo: string = 'Agregar';
  nombreBoton: string = 'Guardar';
  listaEspecialidades: Especialidad[] = [];

  constructor(
    private modal: MatDialogRef<ModalMedicoComponent>,
    @Inject(MAT_DIALOG_DATA) public datosMedico: Medico,
    private fb: FormBuilder,
    private _especialidadServicio: EspecialidadService,
    private _medicoServicio: MedicoService,
    private _compartidoService: CompartidoService
  ) {
    this.formMedico = this.fb.group({
      apellidos: ['', Validators.required],
      nombres: ['', Validators.required],
      direccion: ['', Validators.required],
      telefono: [''],
      genero: ['M', Validators.required],
      especialidadId: ['', Validators.required],
      estado: ['1', Validators.required],
    });

    if (this.datosMedico != null) {
      this.titulo = 'Editar';
      this.nombreBoton = 'Actualizar';
    }

    _especialidadServicio.listaActivos().subscribe({
      next: (data) => {
        if (data.isExitoso) this.listaEspecialidades = data.resultado;
      },
      error: (e) => {},
    });
  }

  ngOnInit(): void {
    if (this.datosMedico != null) {
      this.formMedico.patchValue({
        apellidos: this.datosMedico.apellidos,
        nombres: this.datosMedico.nombres,
        direccion: this.datosMedico.direccion,
        telefono: this.datosMedico.telefono,
        genero: this.datosMedico.genero,
        especialidadId: this.datosMedico.especialidadId,
        estado: this.datosMedico.estado.toString(),
      });
    }
  }

  crearModificarMedico() {
    const medico: Medico = {
      id: this.datosMedico == null ? 0 : this.datosMedico.id,
      apellidos: this.formMedico.value.apellidos,
      nombres: this.formMedico.value.nombres,
      direccion: this.formMedico.value.direccion,
      telefono: this.formMedico.value.telefono,
      genero: this.formMedico.value.genero,
      especialidadId: this.formMedico.value.especialidadId,
      estado: parseInt(this.formMedico.value.estado),
      nombreEspecialidad: '',
    };

    if (this.datosMedico == null) {
      // Crear nuevo médico
      this._medicoServicio.crear(medico).subscribe({
        next: (data) => {
          if (data.isExitoso) {
            this._compartidoService.mostrarAlerta(
              'El médico ha sido creado',
              'Exito!'
            );
            this.modal.close('true');
          } else {
            this._compartidoService.mostrarAlerta(
              'No se pudo crear el médico',
              'Error!'
            );
          }
        },
        error: (e) => {
          this._compartidoService.mostrarAlerta(e.error.errores, "Error");
        }
      });
    } else {
      // Editar médico
      this._medicoServicio.editar(medico).subscribe({
        next:(data) => {
          if (data.isExitoso) {
            this._compartidoService.mostrarAlerta(
              'El médico ha sido actualizado',
              'Exito!'
            );
            this.modal.close('true');
          } else {
            this._compartidoService.mostrarAlerta(
              'No se pudo actualizar el médico',
              'Error!'
            );
          }
        },
        error: (e) => {
          this._compartidoService.mostrarAlerta(e.error.errores,'Error');
        }
      })
    }
  }
}
