// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

package renderers

import (
	_ "image/png"

	"github.com/rs/zerolog/log"
	"github.com/xuri/excelize/v2"
)

func renderDefender(f *excelize.File, data ReportData) {
	if len(data.DefenderData) > 0 {
		_, err := f.NewSheet("Defender")
		if err != nil {
			log.Fatal().Err(err)
		}

		heathers := data.DefenderData[0].GetProperties()

		rows := [][]string{}
		for _, r := range data.DefenderData {
			rows = append(mapToRow(heathers, r.ToMap(data.Mask)), rows...)
		}

		createFirstRow(f, "Defender", heathers)

		currentRow := 4
		for _, row := range rows {
			currentRow += 1
			cell, err := excelize.CoordinatesToCellName(1, currentRow)
			if err != nil {
				log.Fatal().Err(err)
			}
			err = f.SetSheetRow("Defender", cell, &row)
			if err != nil {
				log.Fatal().Err(err)
			}
		}

		configureSheet(f, "Defender", heathers, currentRow)
	} else {
		log.Info().Msg("Skipping Defender. No data to render")
	}
}
