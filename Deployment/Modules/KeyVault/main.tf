data "azurerm_client_config" "current" {}

resource "azurerm_key_vault" "kv" {
  name                        = var.name
  location                    = var.location
  resource_group_name         = var.resource_group_name
  enabled_for_disk_encryption = true
  tenant_id                   = var.tenant_id
  sku_name                    = "standard"
  tags                        = var.tags

}

#access policy for terraform script service account
resource "azurerm_key_vault_access_policy" "kv_access_terraform" {
  key_vault_id = azurerm_key_vault.kv.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  key_permissions = [
    "Create",
    "Get",
  ]

  secret_permissions = [
    "Set",
    "Get",
    "Delete",
    "Recover",
    "Purge"
  ]
}

#access policy for read access (app service)
resource "azurerm_key_vault_access_policy" "kv_read_access" {
  for_each     = var.read_access_objects
  key_vault_id = azurerm_key_vault.kv.id
  tenant_id    = var.tenant_id
  object_id    = each.value

  key_permissions = [
    "List",
    "Get",
  ]

  secret_permissions = [
    "List",
    "Get"
  ]
}

resource "azurerm_key_vault_secret" "passed_in_secrets" {
  count        = length(var.secrets)
  name         = keys(var.secrets)[count.index]
  value        = values(var.secrets)[count.index]
  key_vault_id = azurerm_key_vault.kv.id
  tags         = var.tags

  depends_on = [azurerm_key_vault_access_policy.kv_access_terraform]
}

############################## Keyvalut 2 ######################

resource "azurerm_key_vault" "kv2" {
  name                        = var.name_kv2
  location                    = var.location
  resource_group_name         = var.resource_group_name
  enabled_for_disk_encryption = true
  tenant_id                   = var.tenant_id
  sku_name                    = "standard"
  tags                        = var.tags

  lifecycle {
       prevent_destroy = true
   }

}

#access policy for terraform script service account
resource "azurerm_key_vault_access_policy" "kv2_access_terraform" {
  key_vault_id = azurerm_key_vault.kv2.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  key_permissions = [
    "Create",
    "Get",
  ]

  secret_permissions = [
    "Set",
    "Get",
    "Delete",
    "Recover",
    "Purge"
  ]

  lifecycle {
       prevent_destroy = true
   }
}

#access policy for read access (app service)
resource "azurerm_key_vault_access_policy" "kv2_read_access" {
  for_each     = var.read_access_objects
  key_vault_id = azurerm_key_vault.kv2.id
  tenant_id    = var.tenant_id
  object_id    = each.value

  key_permissions = [
    "List",
    "Get",
  ]

  secret_permissions = [
    "List",
    "Get"
  ]

  lifecycle {
       prevent_destroy = true
   }
}

resource "azurerm_key_vault_secret" "passed_in_secrets_kv2" {
  count        = length(var.secrets_kv2)
  name         = keys(var.secrets_kv2)[count.index]
  value        = values(var.secrets_kv2)[count.index]
  key_vault_id = azurerm_key_vault.kv.id
  tags         = var.tags

  depends_on = [azurerm_key_vault_access_policy.kv2_access_terraform]

  lifecycle {
       prevent_destroy = true
   }
}