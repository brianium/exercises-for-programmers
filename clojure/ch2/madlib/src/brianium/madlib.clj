(ns brianium.madlib
  (:require [brianium.madlib.core :as m :refer [pos]])
  (:gen-class))

;;; Exciting stories!

(defn- format-coll
  [values string]
  (apply format (into [string] values)))

(def hilarious-parts [(pos :noun 3)
                      (pos :verb 1)
                      (pos :adjective 2)
                      (pos :adverb 4)])

(defn hilarious-story []
  (m/exec
   hilarious-parts
   (fn [selections _]
     (format-coll selections "Do you %s your %s %s %s? That's hilarious!"))))

(defn hilarious-story-plus []
  (m/exec
   (into hilarious-parts [(pos :verb 5) (pos :noun 6)])
   (fn [selections _]
     (format-coll selections "Do you %s your %s %s %s? I might %s %s! That's hilarious!"))))

;;; Interactive Adventure!

(def interactive-parts [(pos :noun 1)
                        (fn [[_ value]]
                             (case value
                               "wizard"  [:noun 2]
                               "warrior" [:verb 2]
                               [:adjective 2]))])

(def interactive-templates
  {[:noun :noun] "I am a great %s!! I seek the ancient %s!"
   [:noun :verb] "I am a mighty %s!! I will %s evil where I find it!"
   [:noun :adjective] "I am just a %s trying to be %s."})

(defn interactive-story []
  (m/exec
   interactive-parts
   (fn [selections answers]
     (->> answers
          (mapv ffirst)
          (get interactive-templates)
          (format-coll selections)))))

;;; Run it!

(defn -main [& args]
  (case (first args)
    "1" (hilarious-story)
    "2" (hilarious-story-plus)
    "3" (interactive-story)
    (hilarious-story)))
