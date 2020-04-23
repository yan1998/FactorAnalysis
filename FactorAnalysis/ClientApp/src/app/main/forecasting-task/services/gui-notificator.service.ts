import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


@Injectable({
  providedIn: 'root'
})
export class GuiNotificatorService {
  private _options: any;
  private _title: string;

  constructor(private _toastr: ToastrService) {
      this._options = {
        closeButton: true,
        enableHtml: true
      };
  }

  showSuccess(message: string): void {
    this._title = 'Успех';
    this._toastr.success(message, this._title, this._options);
  }

  showError(message: string): void {
    this._title = 'Ошибка';
    this._toastr.error(message, this._title, this._options);
  }

  showWarning(message: string): void {
    this._title = 'Предупреждение';
    this._toastr.error(message, this._title, this._options);
  }

  showInfo(message: string): void {
    this._title = 'Уведомление';
    this._toastr.info(message, this._title, this._options);
  }
}
