import { Component, Inject, OnInit} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { ExchangeRateFactors } from '../../models/exchange-rate-factors';

@Component({
  selector: 'app-exchange-rate-factors-dialog',
  templateUrl: './exchange-rate-factors-dialog.component.html',
  styleUrls: ['./exchange-rate-factors-dialog.component.css']
})
export class ExchangeRateFactorsDialogComponent implements OnInit {

  date: FormControl;
  maxDate = new Date();
  isEditMode: boolean;
  exchangeRateFactorsForm = new FormGroup({
    creditRate: new FormControl('', [Validators.required]),
    gdpIndicator: new FormControl('', [Validators.required]),
    importIndicator: new FormControl('', [Validators.required]),
    exportIndicator: new FormControl('', [Validators.required]),
    inflationIndex: new FormControl('', [Validators.required]),
    exchangeRateUSD: new FormControl('', [Validators.required]),
    exchangeRateEUR: new FormControl('', [Validators.required])
  });

  constructor(
    public dialogRef: MatDialogRef<ExchangeRateFactorsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  ngOnInit() {
    this.isEditMode = this.data.mode === 'Edit';
    if (this.isEditMode) {
      this.exchangeRateFactorsForm.get('creditRate').setValue(this.data.entity.creditRate);
      this.exchangeRateFactorsForm.get('gdpIndicator').setValue(this.data.entity.gdpIndicator);
      this.exchangeRateFactorsForm.get('importIndicator').setValue(this.data.entity.importIndicator);
      this.exchangeRateFactorsForm.get('exportIndicator').setValue(this.data.entity.exportIndicator);
      this.exchangeRateFactorsForm.get('inflationIndex').setValue(this.data.entity.inflationIndex);
      this.exchangeRateFactorsForm.get('exchangeRateUSD').setValue(this.data.entity.exchangeRateUSD);
      this.exchangeRateFactorsForm.get('exchangeRateEUR').setValue(this.data.entity.exchangeRateEUR);
      this.date = new FormControl(new Date(this.data.entity.date));
    } else {
      this.date = new FormControl(new Date());
    }
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }

  onSubmitClick(): void {
    const result: ExchangeRateFactors = {
      id: 0,
      date: this.date.value,
      exchangeRateUSD: this.exchangeRateFactorsForm.get('exchangeRateUSD').value,
      exchangeRateEUR: this.exchangeRateFactorsForm.get('exchangeRateEUR').value,
      creditRate: this.exchangeRateFactorsForm.get('creditRate').value,
      gdpIndicator: this.exchangeRateFactorsForm.get('gdpIndicator').value,
      importIndicator: this.exchangeRateFactorsForm.get('importIndicator').value,
      exportIndicator: this.exchangeRateFactorsForm.get('exportIndicator').value,
      inflationIndex: this.exchangeRateFactorsForm.get('inflationIndex').value
    };
    if (this.isEditMode) {
      result.id = this.data.entity.id;
    }
    this.dialogRef.close(result);
  }

}
