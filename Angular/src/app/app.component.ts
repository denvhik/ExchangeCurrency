import { Component } from '@angular/core';
import {ExchangeServiceService} from 'src/app/Service/exchange-service.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Angular';
  sourceCurrency: string = '';
  amount: number = 0;
  targetCurrency: string = '';
  convertedAmount: number = 0;

  constructor(private exchangeServiceService: ExchangeServiceService) { }

  async convertCurrency() {
    try {
      this.convertedAmount = await this.exchangeServiceService.convertCurrency(this.sourceCurrency, this.amount, this.targetCurrency);
    } catch (error) {
      console.log(this.convertedAmount)
     console.log (error)
      this.convertedAmount = this.convertedAmount;
    }
  }
}
