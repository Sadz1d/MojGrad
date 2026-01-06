// services/export.service.ts - KOMPLETNO ISPRAVLJEN
import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';

export interface ExportOptions {
  fileName?: string;
  sheetName?: string;
  dateFormat?: string;
  includeHeaders?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ExportService {
  
  /**
   * Exportuje niz objekata u Excel fajl
   */
  exportToExcel<T extends Record<string, any>>( // 👈 DODAJ extends Record<string, any>
    data: T[],
    options: ExportOptions = {}
  ): void {
    const {
      fileName = 'export.xlsx',
      sheetName = 'Sheet1',
      dateFormat = 'DD.MM.YYYY',
      includeHeaders = true
    } = options;

    if (!data || data.length === 0) {
      console.error('No data to export');
      return;
    }

    // Type guard za prazan objekat
    const sample = data[0] || {};
    const headers = includeHeaders ? Object.keys(sample) : undefined;

    // Kreiraj worksheet
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data, {
      header: headers,
      dateNF: dateFormat
    });

    // Kreiraj workbook
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, sheetName);

    // Generiši Excel fajl
    const excelBuffer: any = XLSX.write(wb, {
      bookType: 'xlsx',
      type: 'array'
    });

    // Sačuvaj fajl
    this.saveAsExcelFile(excelBuffer, fileName);
  }

  /**
   * Exportuje HTML tabelu u Excel
   */
  exportTableToExcel(
    tableId: string,
    fileName: string = 'table-export.xlsx',
    sheetName: string = 'Sheet1'
  ): void {
    const table = document.getElementById(tableId);
    if (!table) {
      console.error(`Table with id '${tableId}' not found`);
      return;
    }

    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(table);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, sheetName);

    const excelBuffer: any = XLSX.write(wb, {
      bookType: 'xlsx',
      type: 'array'
    });

    this.saveAsExcelFile(excelBuffer, fileName);
  }

  /**
   * Exportuje podatke u CSV format
   */
  exportToCSV<T extends Record<string, any>>( // 👈 ISTO OVDJE
    data: T[],
    fileName: string = 'export.csv',
    delimiter: string = ','
  ): void {
    if (!data || data.length === 0) {
      console.error('No data to export');
      return;
    }

    // Kreiraj header
    const headers = Object.keys(data[0] || {});
    const csvRows = [];

    // Dodaj header
    csvRows.push(headers.join(delimiter));

    // Dodaj podatke
    for (const row of data) {
      const values = headers.map(header => {
        const value = row[header];
        
        // Formatiraj datum
        if (value instanceof Date) {
          return this.formatDate(value);
        }
        
        // Escape-uj delimiter i quotes
        const escaped = String(value || '')
          .replace(/"/g, '""')
          .replace(/\n/g, ' ');
        
        // Dodaj quotes ako sadrži delimiter ili quotes
        return escaped.includes(delimiter) || escaped.includes('"') 
          ? `"${escaped}"` 
          : escaped;
      });
      
      csvRows.push(values.join(delimiter));
    }

    // Kreiraj CSV string
    const csvString = csvRows.join('\n');
    
    // Kreiraj blob i sačuvaj
    const blob = new Blob([csvString], { type: 'text/csv;charset=utf-8;' });
    saveAs(blob, fileName);
  }

  /**
   * Exportuje sa advanced opcijama (stilovi, više sheetova)
   */
  exportAdvanced<T extends Record<string, any>>( // 👈 I OVDJE
    data: {
      sheetName: string;
      data: T[];
      columns?: string[];
      columnWidths?: number[];
    }[],
    fileName: string = 'advanced-export.xlsx'
  ): void {
    const wb: XLSX.WorkBook = XLSX.utils.book_new();

    data.forEach((sheetConfig, index) => {
      const sample = sheetConfig.data[0] || {};
      const headers = sheetConfig.columns || Object.keys(sample);

      const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(
        sheetConfig.data,
        { header: headers }
      );

      // Podesi širinu kolona ako je dato
      if (sheetConfig.columnWidths) {
        const cols = sheetConfig.columnWidths.map(width => ({ width }));
        ws['!cols'] = cols;
      }

      XLSX.utils.book_append_sheet(
        wb, 
        ws, 
        sheetConfig.sheetName || `Sheet${index + 1}`
      );
    });

    const excelBuffer: any = XLSX.write(wb, {
      bookType: 'xlsx',
      type: 'array'
    });

    this.saveAsExcelFile(excelBuffer, fileName);
  }

  /**
   * Private helper metoda za čuvanje fajla
   */
  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8'
    });
    
    saveAs(data, fileName);
  }

  /**
   * Formatira datum za CSV
   */
  private formatDate(date: Date): string {
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
  }
}