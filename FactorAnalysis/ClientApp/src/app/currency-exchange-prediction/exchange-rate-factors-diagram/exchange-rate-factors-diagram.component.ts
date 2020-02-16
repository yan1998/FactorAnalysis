import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, BaseChartDirective, Label } from 'ng2-charts';
import * as pluginAnnotations from 'chartjs-plugin-annotation';
import { ExchangeRateFactorsService } from '../services/exchange-rate-factors.service';
import { ExchangeRateFactors } from '../models/exchange-rate-factors';
import { FormControl } from '@angular/forms';
import { ExchangeRateFactorsEnum } from '../models/enums/exchange-rate-factors.enum';

@Component({
  selector: 'app-exchange-rate-factors-diagram',
  templateUrl: './exchange-rate-factors-diagram.component.html'
})
export class ExchangeRateFactorsDiagramComponent implements OnInit {

  public lineChartData: ChartDataSets[] = [
    { data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A' },
  ];
  public lineChartLabels: Label[];
  public lineChartOptions: (ChartOptions & { annotation: any }) = {
    responsive: true,
    scales: {
      // We use this empty structure as a placeholder for dynamic theming.
      xAxes: [{}],
      yAxes: [
        {
          id: 'y-axis-0',
          position: 'left',
          ticks: {
            beginAtZero: true
          }
        }
      ],
    },
    annotation: {
      annotations: [
        {
          type: 'line',
          mode: 'vertical',
          scaleID: 'x-axis-0',
          value: 'March',
          borderColor: 'orange',
          borderWidth: 2,
          label: {
            enabled: true,
            fontColor: 'orange',
            content: 'LineAnno'
          }
        },
      ],
    },
  };
  public lineChartColors: Color[] = [
    { // grey
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }
  ];
  public lineChartLegend = true;
  public lineChartType = 'line';
  public lineChartPlugins = [pluginAnnotations];
  public dateFrom: FormControl;
  public dateTo: FormControl;
  public selectedFactor: string;
  public options: string[];
  public exchangeRateFactors: ExchangeRateFactors[];

  @ViewChild(BaseChartDirective, { static: true }) chart: BaseChartDirective;

  constructor(private _exchangeRateFactorsService: ExchangeRateFactorsService) { }

  ngOnInit() {
    this.options = Object.keys(ExchangeRateFactorsEnum);
    this.options = this.options.slice(this.options.length / 2);
    this.dateFrom = new FormControl(new Date(2000, 0, 1));
    this.dateTo = new FormControl(new Date(2001, 0, 1));
  }

  getExchangeRateFactorsRange(dateFrom: Date, dateTo: Date) {
    this._exchangeRateFactorsService.getExchangeRateFactorsRange(dateFrom, dateTo)
    .subscribe(result => {
      this.exchangeRateFactors = result;
      this.processExchangeRateFactorsResponse(result);
    }, error => console.error(error));
  }

  processExchangeRateFactorsResponse(response: ExchangeRateFactors[]) {
    const selectedFactor = ExchangeRateFactorsEnum[this.selectedFactor];
    this.lineChartData = [];
    this.lineChartLabels = [];
    const chartDataSet: ChartDataSets = { label: '', data: []};
    response.forEach(element => {
      const date = new Date(element.date);
      this.lineChartLabels.push(`${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`);
      switch (selectedFactor) {
        case ExchangeRateFactorsEnum.creditRate:
          chartDataSet.data.push(element.creditRate);
          chartDataSet.label = 'creditRate';
          break;
        case ExchangeRateFactorsEnum.exchangeRateEUR:
          chartDataSet.data.push(element.exchangeRateEUR);
          chartDataSet.label = 'exchangeRateEUR';
          break;
        case ExchangeRateFactorsEnum.exchangeRateUSD:
          chartDataSet.data.push(element.exchangeRateUSD);
          chartDataSet.label = 'exchangeRateUSD';
          break;
        case ExchangeRateFactorsEnum.exportIndicator:
          chartDataSet.data.push(element.exportIndicator);
          chartDataSet.label = 'exportIndicator';
          break;
        case ExchangeRateFactorsEnum.gdpIndicator:
          chartDataSet.data.push(element.gdpIndicator);
          chartDataSet.label = 'gdpIndicator';
          break;
        case ExchangeRateFactorsEnum.importIndicator:
          chartDataSet.data.push(element.importIndicator);
          chartDataSet.label = 'importIndicator';
          break;
        case ExchangeRateFactorsEnum.inflationIndex:
          chartDataSet.data.push(element.inflationIndex);
          chartDataSet.label = 'inflationIndex';
          break;
      }
    });
    this.lineChartData.push(chartDataSet);
  }

  public changedFactor() {
    this.processExchangeRateFactorsResponse(this.exchangeRateFactors);
  }

  public getData() {
    this.getExchangeRateFactorsRange(this.dateFrom.value, this.dateTo.value);
  }
}
