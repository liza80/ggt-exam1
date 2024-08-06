var app = angular.module('githubApp', []);

app.controller('GitHubController', function ($scope, $http) {
    $scope.searchTerm = '';
    $scope.repositories = [];
    $scope.showEmailPopup = false;
    $scope.emailAddress = '';
    $scope.selectedRepo = null;

    $scope.search = function () {
        $http.get('https://api.github.com/search/repositories?q=' + $scope.searchTerm)
            .then(function (response) {
                $scope.repositories = response.data.items;
            }, function (error) {
                console.error('Error fetching repositories:', error);
            });
    };

    $scope.bookmark = function (repo) {
        $http.post('/api/github/bookmark', repo)
            .then(function (response) {
                alert('Repository bookmarked!');
            }, function (error) {
                console.error('Error bookmarking repository:', error);
            });
    };

    $scope.openEmailPopup = function (repo) {
        $scope.selectedRepo = repo;
        $scope.showEmailPopup = true;
    };

    $scope.closeEmailPopup = function () {
        $scope.showEmailPopup = false;
        $scope.emailAddress = '';
        $scope.selectedRepo = null;
    };

    $scope.sendEmail = function () {
        var emailRequest = {
            EmailAddress: $scope.emailAddress,
            Repository: $scope.selectedRepo
        };

        $http.post('/api/github/sendemail', emailRequest)
            .then(function (response) {
                alert('Email sent!');
                $scope.closeEmailPopup();
            }, function (error) {
                console.error('Error sending email:', error);
            });
    };
});
