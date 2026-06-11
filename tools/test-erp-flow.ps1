$ErrorActionPreference = "Stop"

$baseUrl = if ($env:PVC_ERP_API_URL) { $env:PVC_ERP_API_URL } else { "http://localhost:5006/api" }

function Post-Json($path, $body) {
    Invoke-RestMethod -Method Post -Uri "$baseUrl$path" -ContentType "application/json" -Body ($body | ConvertTo-Json -Depth 10)
}

function Get-Json($path) {
    Invoke-RestMethod -Method Get -Uri "$baseUrl$path"
}

Write-Host "Testing PVC ERP API at $baseUrl"

$suffix = Get-Date -Format "yyyyMMddHHmmss"

$calcium = Post-Json "/inventory/raw-materials" @{ name = "Calcium Test $suffix"; unit = "Kg"; reorderLevel = 100 }
$resin = Post-Json "/inventory/raw-materials" @{ name = "Resin Test $suffix"; unit = "Kg"; reorderLevel = 50 }
$pulverizer = Post-Json "/inventory/raw-materials" @{ name = "Pulverizer Test $suffix"; unit = "Kg"; reorderLevel = 40 }
Write-Host "Raw materials created"

$supplier = Post-Json "/inventory/suppliers" @{ name = "Test Supplier $suffix"; country = "India"; gstNumber = "TESTGST$suffix"; phone = "9999999999" }
$grn = Post-Json "/inventory/grns" @{
    supplierId = $supplier.id
    receiptDate = "2026-05-30"
    qualityStatus = 2
    items = @(
        @{ rawMaterialId = $calcium.id; batchNumber = "CA-BATCH-$suffix"; quantityKg = 500 },
        @{ rawMaterialId = $resin.id; batchNumber = "RE-BATCH-$suffix"; quantityKg = 300 },
        @{ rawMaterialId = $pulverizer.id; batchNumber = "PU-BATCH-$suffix"; quantityKg = 200 }
    )
}
Write-Host "GRN created: $($grn.grnNumber)"

$formula = Post-Json "/production/formulas" @{
    formulaCode = "PVC-STD-$suffix"
    description = "Standard PVC test formula"
    items = @(
        @{ rawMaterialId = $calcium.id; standardQuantityKg = 70 },
        @{ rawMaterialId = $resin.id; standardQuantityKg = 35 },
        @{ rawMaterialId = $pulverizer.id; standardQuantityKg = 20 }
    )
}

$machine = Post-Json "/production/machines" @{ machineNumber = "M-$suffix"; description = "Extruder test machine" }
$barrel = Post-Json "/production/screw-barrels" @{ barrelNumber = "SB-$suffix"; type = "Standard"; targetKgPerHour = 80 }

$batch = Post-Json "/production/batches" @{
    batchFormulaId = $formula.id
    productionDate = "2026-05-30"
    shift = "A"
    operatorName = "Test Operator"
    supervisorName = "Test Supervisor"
    consumptions = @(
        @{ rawMaterialId = $calcium.id; standardQuantityKg = 70; actualQuantityKg = 72 },
        @{ rawMaterialId = $resin.id; standardQuantityKg = 35; actualQuantityKg = 34 },
        @{ rawMaterialId = $pulverizer.id; standardQuantityKg = 20; actualQuantityKg = 19 }
    )
    outputs = @(
        @{ machineId = $machine.id; screwBarrelId = $barrel.id; stage = 1; pipeDimension = "63mm"; approvedQuantityKg = 100; rejectedQuantityKg = 8; hoursRun = 2 },
        @{ machineId = $machine.id; screwBarrelId = $barrel.id; stage = 2; pipeDimension = "63mm Socket"; approvedQuantityKg = 95; rejectedQuantityKg = 5; hoursRun = 1 }
    )
}
Write-Host "Production batch created: $($batch.batchNumber)"

$scrap = Post-Json "/production/scrap" @{
    stage = 1
    scrapType = "Damaged Pipes"
    generatedKg = 13
    reusedKg = 9
    sourceReference = $batch.batchNumber
    recordDate = "2026-05-30"
}
Write-Host "Scrap recorded, recovery: $($scrap.recoveryPercent)%"

$customer = Post-Json "/dispatch/customers" @{ name = "Test Customer $suffix"; gstNumber = "CUSTGST$suffix"; phone = "8888888888"; address = "Test Address" }
$dispatch = Post-Json "/dispatch/challans" @{
    customerId = $customer.id
    dispatchDate = "2026-05-30"
    vehicleNumber = "GJ01TEST"
    items = @(
        @{ pipeDimension = "63mm"; quantityKg = 80; quantityPieces = 100 }
    )
}

$invoice = Post-Json "/dispatch/invoices" @{
    dispatchChallanId = $dispatch.id
    invoiceDate = "2026-05-30"
    taxableAmount = 10000
    gstAmount = 1800
}
Write-Host "Invoice created: $($invoice.invoiceNumber)"

Write-Host "`nStock:"
Get-Json "/inventory/stock" | Format-Table name, availableKg, reorderLevel

Write-Host "`nScrew barrel efficiency:"
Get-Json "/production/screw-barrel-efficiency" | Format-Table barrelNumber, producedKg, hoursRun, kgPerHour, targetKgPerHour

Write-Host "`nDashboard:"
Get-Json "/reports/dashboard" | Format-List

Write-Host "`nAll module checks completed."
