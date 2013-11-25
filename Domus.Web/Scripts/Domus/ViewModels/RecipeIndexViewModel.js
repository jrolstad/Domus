function RecipeIndexViewModel() {
    var self = this;

    self.searchText = ko.observable("");
    self.searchResults = ko.observableArray();

    self.executeSearch = function() {
        self.searchResults = ko.observableArray([
            { Name: "Josh" },
            { Name: "Bethany"}
        ]);
    };

    self.addNewRecipe = function() {

    };
}