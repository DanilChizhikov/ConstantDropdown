# Changelog

## [1.2.1] - 2026-09-26

### Changes

- renamed scope to 'com.dtech'

## [1.2.0] - 2026-09-25

### Changes

- support multiple `[ConstantSource]` with the same linking type: values from all sources are merged into one dropdown and grouped by source name (`TypeName/...` for classes, `DeclaringType.FieldName/...` for collection fields). Previously only the first found source was used
- keys that clash after merge are logged as a warning and skipped
- package files moved to repository root

## [1.1.0] - 2025-09-27

### Changes

- support collections
- fixed dropdown drawing on sub classes and structs

## [1.0.0] - 2025-09-24

This release is the first and it carries:

- The basic logic for drawing the dropdown list
- Saving selected values
- Basic classes for the editor
