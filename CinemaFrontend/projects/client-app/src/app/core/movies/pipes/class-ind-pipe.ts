import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'classInd',
})
export class ClassIndPipe implements PipeTransform {
  transform(value: string): string {
    switch (value) {
      case 'All':
        return 'class-ind/classificacao_indicativa_livre.svg';
      case 'Rated6Plus':
        return 'class-ind/classificacao_indicativa_6_anos.svg';
      case 'Rated10Plus':
        return 'class-ind/classificacao_indicativa_10_anos.svg';
      case 'Rated12Plus':
        return 'class-ind/classificacao_indicativa_12_anos.svg';
      case 'Rated14Plus':
        return 'class-ind/classificacao_indicativa_14_anos.svg';
      case 'Rated16Plus':
        return 'class-ind/classificacao_indicativa_16_anos.svg';
      case 'Rated18Plus':
        return 'class-ind/classificacao_indicativa_18_anos.svg';
      default:
        return '';
    }
  }
}
