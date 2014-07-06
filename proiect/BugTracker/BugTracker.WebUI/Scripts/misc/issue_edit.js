$(document).ready(function () {
    tagInput = $('#magicSuggest').magicSuggest({
        allowFreeEntries: true,
        cls: "form-control",
        data: allTags,
        valueField: "name"
    }).setValue(selectedTags);
});