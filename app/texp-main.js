var appTexp = angular.module("TExpApp", [
    "ngMessages", "ngAnimate", "ui.bootstrap","checklist-model",
    "toastr", "angular-loading-bar", 'flow', 'server-validate'
]);

////////// sorting by name of array member \\\\\\\\\\\\\\\\\\\\\\
function sortByProperty(array, propertyName) {
    console.log("sortByProperty propertyName=" + propertyName);
    return array.sort(function(a, b) {
        if (a[propertyName] > b[propertyName])
            return 1;
        else if (a[propertyName] < b[propertyName]) return -1;
        else return 0;
    });
}